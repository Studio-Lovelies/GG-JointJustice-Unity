using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using SaveFiles;
using UnityEngine;

public class ActionDecoder
{
    public event Action OnActionDone;
    public IActorController ActorController { get; set; }
    public ISceneController SceneController { get; set; }
    public IAudioController AudioController { get; set; }
    public IEvidenceController EvidenceController { get; set; }
    public IAppearingDialogueController AppearingDialogueController { get; set; }
    public IDialogueController DialogueController { get; set; }
    public IPenaltyManager PenaltyManager { get; set; }

    /// <summary>
    ///     Parse action lines inside from inside .ink files
    /// </summary>
    /// <param name="actionLine">Line of a .ink file that starts with &amp; (and thereby is not a "spoken dialogue" line)</param>
    /// <remarks>
    ///     Writers are able to call methods inside .ink files. This is done by using the following syntax:
    ///     <code>
    ///     &amp;{methodName}:{parameter1},{parameter2},...
    ///     </code>
    ///     This method is responsible for:
    ///         1. Finding a method inside this class, matching `methodName`
    ///         2. Verifying the amount of parameters matches the amount of parameters needed in the method
    ///         3. Attempting to parse each parameter into the correct type using <see cref="Parser&lt;T&gt;"/> of the type
    ///         4. Invoking the method with the parsed parameters
    /// </remarks>
    public void OnNewActionLine(string actionLine)
    {
        actionLine = actionLine.Trim();
        const char actionSideSeparator = ':';
        const char actionParameterSeparator = ',';

        var actionNameAndParameters = actionLine.Substring(1, actionLine.Length - 1).Trim().Split(actionSideSeparator);

        if (actionNameAndParameters.Length > 2)
        {
            throw new TextDecoder.Parser.ScriptParsingException($"More than one '{actionSideSeparator}' detected in line '{actionLine}'");
        }

        var action = actionNameAndParameters[0];
        var parameters = (actionNameAndParameters.Length == 2) ? actionNameAndParameters[1].Split(actionParameterSeparator) : Array.Empty<string>();

        // Find method with exact same name as action inside script
        var method = GetType().GetMethod(action, BindingFlags.Instance | BindingFlags.NonPublic);
        if (method == null)
        {
            throw new TextDecoder.Parser.ScriptParsingException($"DirectorActionDecoder contains no method named '{action}'");
        }

        var methodParameters = method.GetParameters();
        var optionalParameters = methodParameters.Count(parameter => parameter.IsOptional);
        if (parameters.Length < (methodParameters.Length - optionalParameters) || parameters.Length > (methodParameters.Length))
        {
            throw new TextDecoder.Parser.ScriptParsingException($"'{action}' requires {(optionalParameters == 0 ? "exactly" : "between")} {(optionalParameters == 0 ? methodParameters.Length.ToString() : $"{methodParameters.Length-optionalParameters} and {methodParameters.Length}")} parameters (has {parameters.Length} instead)");
        }

        var parsedMethodParameters = new List<object>();
        // For each supplied parameter of that action...
        for (var index = 0; index < parameters.Length; index++)
        {
            if (parameters.Length <= index && methodParameters[index].IsOptional)
            {
                parsedMethodParameters.Add(methodParameters[index].DefaultValue);
            }

            // Determine it's type
            var methodParameter = methodParameters[index];

            // Edge-case for enums
            if (methodParameter.ParameterType.BaseType == typeof(Enum))
            {
                try
                {
                    parsedMethodParameters.Add(Enum.Parse(methodParameter.ParameterType, parameters[index]));
                    continue;
                }
                catch (ArgumentException e)
                {
                    var pattern = new Regex(@"Requested value '(.*)' was not found\.");
                    var match = pattern.Match(e.Message);
                    if (match.Success)
                    {
                        throw new TextDecoder.Parser.ScriptParsingException($"'{parameters[index]}' is incorrect as parameter #{index + 1} ({methodParameter.Name}) for action '{action}': Cannot convert '{match.Groups[1].Captures[0]}' into an {methodParameter.ParameterType} (valid values include: '{string.Join(", ", Enum.GetValues(methodParameter.ParameterType).Cast<object>().Select(a=>a.ToString()))}')");
                    }

                    if (e.Message == "Must specify valid information for parsing in the string.")
                    {
                        throw new TextDecoder.Parser.ScriptParsingException($"'' is incorrect as parameter #{index + 1} ({methodParameter.Name}) for action '{action}': Cannot convert '' into an {methodParameter.ParameterType} (valid values include: '{string.Join(", ", Enum.GetValues(methodParameter.ParameterType).Cast<object>().Select(a => a.ToString()))}')");
                    }
                    throw;
                }
            }

            // Construct a parser for it
            var parser = GetType().Assembly.GetTypes().FirstOrDefault(type => type.BaseType is { IsGenericType: true } && type.BaseType.GenericTypeArguments[0] == methodParameter.ParameterType);
            if (parser == null)
            {
                Debug.LogError($"The TextDecoder.Parser namespace contains no Parser for type {methodParameter.ParameterType}");
                return;
            }

            var parserConstructor = parser.GetConstructor(Type.EmptyTypes);
            if (parserConstructor == null)
            {
                Debug.LogError($"TextDecoder.Parser for type {methodParameter.ParameterType} has no constructor without parameters");
                return;
            }

            // Find the 'Parse' method on that parser
            var parseMethod = parser.GetMethod("Parse");
            if (parseMethod == null)
            {
                Debug.LogError($"TextDecoder.Parser for type {methodParameter.ParameterType} has no 'Parse' method");
                return;
            }

            // Create a parser and call the 'Parse' method
            var parserInstance = parserConstructor.Invoke(Array.Empty<object>());
            object[] parseMethodParameters = { parameters[index], null };

            // If we received an error attempting to parse a parameter to the type, expose it to the user
            var humanReadableParseError = parseMethod.Invoke(parserInstance, parseMethodParameters);
            if (humanReadableParseError != null)
            {
                throw new TextDecoder.Parser.ScriptParsingException($"'{parameters[index]}' is incorrect as parameter #{index + 1} ({methodParameter.Name}) for action '{action}': {humanReadableParseError}");
            }

            parsedMethodParameters.Add(parseMethodParameters[1]);
        }

        // If the method supports optional parameters, fill the remaining parameters based on the default value of the method
        for (var suppliedParameterCount = parameters.Length; suppliedParameterCount < methodParameters.Length; suppliedParameterCount++)
        {
            parsedMethodParameters.Add(methodParameters[suppliedParameterCount].DefaultValue);
        }

        // Call the method
        method.Invoke(this, parsedMethodParameters.ToArray());
    }

    // ReSharper disable InconsistentNaming
    // ReSharper disable UnusedMember.Local
#pragma warning disable IDE0051 // Remove unused private members
    #region AppearingDialogueController
    /// <summary>Makes regular letters take the given amount of seconds before showing the next letter in dialogue.</summary>
    /// <param name="characterDelay">Time in seconds, use `.` (not `,`) for decimal places.</param>
    /// <example>&amp;DIALOGUE_SPEED:1.05</example>
    /// <example>&amp;DIALOGUE_SPEED:0.2</example>
    /// <example>&amp;DIALOGUE_SPEED:0.05</example>
    /// <category>Dialogue</category>
    private void DIALOGUE_SPEED(float characterDelay)
    {
        AppearingDialogueController.CharacterDelay = characterDelay;
        OnActionDone?.Invoke();
    }

    /// <summary>Makes punctuation take the given amount of seconds before showing the next letter in dialogue.</summary>
    /// <param name="seconds">Time in seconds, use `.` (not `,`) for decimal places.</param>
    /// <example>&amp;PUNCTUATION_SPEED:1.05</example>
    /// <example>&amp;PUNCTUATION_SPEED:0.2</example>
    /// <example>&amp;PUNCTUATION_SPEED:0.05</example>
    /// <category>Dialogue</category>
    private void PUNCTUATION_SPEED(float seconds)
    {
        AppearingDialogueController.DefaultPunctuationDelay = seconds;
        OnActionDone?.Invoke();
    }

    /// <summary>Starts or stops autoskipping of dialogue, where it automatically continues after it is done.</summary>
    /// <param name="value">Set to either `true` or `false` to enable or disable automatic dialogue skipping respectively.</param>
    /// <example>&amp;AUTO_SKIP:true</example>
    /// <example>&amp;AUTO_SKIP:false</example>
    /// <category>Dialogue</category>
    private void AUTO_SKIP(bool value)
    {
        AppearingDialogueController.AutoSkip = value;
        OnActionDone?.Invoke();
    }

    /// <summary>Disables or enables text speedup. Enabled by default.</summary>
    /// <param name="value">Set to either `true` or `false` to not speedup or speedup text respectively.</param>
    /// <example>&amp;DISABLE_SKIPPING:true</example>
    /// <example>&amp;DISABLE_SKIPPING:false</example>
    /// <category>Dialogue</category>
    private void DISABLE_SKIPPING(bool value)
    {
        AppearingDialogueController.SkippingDisabled = value;
        OnActionDone?.Invoke();
    }

    /// <summary>Makes the next dialogue add to the current one instead of replacing it.</summary>
    /// <example>&amp;CONTINUE_DIALOGUE</example>
    /// <category>Dialogue</category>
    private void CONTINUE_DIALOGUE()
    {
        AppearingDialogueController.ContinueDialogue = true;
        OnActionDone?.Invoke();
    }

    /// <summary>Makes the next line of dialogue appear all at once, instead of character by character.</summary>
    /// <category>Dialogue</category>
    /// <example>&amp;APPEAR_INSTANTLY</example>
    private void APPEAR_INSTANTLY()
    {
        AppearingDialogueController.AppearInstantly = true;
        OnActionDone?.Invoke();
    }

    /// <summary>Hides the dialogue textbox until the next line of dialogue.</summary>
    /// <category>Dialogue</category>
    /// <example>&amp;HIDE_TEXTBOX</example>
    private void HIDE_TEXTBOX()
    {
        AppearingDialogueController.TextBoxHidden = true;
        OnActionDone?.Invoke();
    }
    #endregion

    #region EvidenceController
    /// <summary>Adds the provided evidence to the court record.</summary>
    /// <param name="evidence" validFiles="Assets/Resources/Evidence/*.asset">Name of evidence to add</param>
    /// <example>&amp;ADD_EVIDENCE:Bent_Coins</example>
    /// <category>Evidence</category>
    private void ADD_EVIDENCE(EvidenceAssetName evidence)
    {
        EvidenceController.AddEvidence(evidence);
        OnActionDone?.Invoke();
    }

    /// <summary>Removes the provided evidence from the court record.</summary>
    /// <param name="evidence" validFiles="Assets/Resources/Evidence/*.asset">Name of evidence to remove</param>
    /// <example>&amp;REMOVE_EVIDENCE:Bent_Coins</example>
    /// <category>Evidence</category>
    private void REMOVE_EVIDENCE(EvidenceAssetName evidence)
    {
        EvidenceController.RemoveEvidence(evidence);
        OnActionDone?.Invoke();
    }

    /// <summary>Adds the provided actor to the court record.</summary>
    /// <param name="actorName" validFiles="Assets/Resources/Actors/*.asset">Name of the actor to add to the court record</param>
    /// <example>&amp;ADD_RECORD:Jory</example>
    /// <category>Evidence</category>
    private void ADD_RECORD(ActorAssetName actorName)
    {
        EvidenceController.AddToCourtRecord(actorName);
        OnActionDone?.Invoke();
    }

    /// <summary>Forces the evidence menu open and doesn't continue the story until the player presents evidence.</summary>
    /// <example>&amp;PRESENT_EVIDENCE</example>
    /// <category>Evidence</category>
    private void PRESENT_EVIDENCE()
    {
        EvidenceController.RequirePresentEvidence();
    }

    /// <summary>Substitutes the provided evidence for their substitute.</summary>
    /// <param name="initialEvidenceName" validFiles="Assets/Resources/Evidence/*.asset">Name of evidence to replace with the substitute</param>
    /// <param name="substituteEvidenceName" validFiles="Assets/Resources/Evidence/*.asset">Name of the substitute evidence</param>
    /// <example>&amp;SUBSTITUTE_EVIDENCE:Plumber_Invoice,Bent_Coins</example>
    /// <category>Evidence</category>
    private void SUBSTITUTE_EVIDENCE(EvidenceAssetName initialEvidenceName, EvidenceAssetName substituteEvidenceName)
    {
        EvidenceController.SubstituteEvidence(initialEvidenceName, substituteEvidenceName);
        OnActionDone?.Invoke();
    }
    #endregion

    #region AudioController
    /// <summary>Plays provided SFX.</summary>
    /// <param name="sfx" validFiles="Assets/Resources/Audio/SFX/*.wav">Filename of a sound effect</param>
    /// <example>&amp;PLAY_SFX:EvidenceShoop</example>
    /// <category>Audio</category>
    private void PLAY_SFX(SfxAssetName sfx)
    {
        AudioController.PlaySfx(sfx);
        OnActionDone?.Invoke();
    }

    /// <summary>Plays the provided song. Stops the current one. Loops infinitely.</summary>
    /// <param name="songName" validFiles="Assets/Resources/Audio/Music/*.mp3">Filename of a song</param>
    /// <example>&amp;PLAY_SONG:TurnaboutGrumpsters</example>
    /// <category>Audio</category>
    private void PLAY_SONG(SongAssetName songName)
    {
        AudioController.PlaySong(songName);
        OnActionDone?.Invoke();
    }

    /// <summary>If music is currently playing, stop it.</summary>
    /// <example>&amp;STOP_SONG</example>
    /// <category>Audio</category>
    private void STOP_SONG()
    {
        AudioController.StopSong();
        OnActionDone?.Invoke();
    }
    #endregion

    #region SceneController
    /// <summary>Fades the screen to black, only works if not faded out.</summary>
    /// <param name="timeInSeconds">number of seconds for the fade out to take. Decimal numbers allowed</param>
    /// <example>&amp;FADE_OUT:1</example>
    /// <category>Scene</category>
    private void FADE_OUT(float timeInSeconds)
    {
        SceneController.FadeOut(timeInSeconds);
    }

    /// <summary>Fades the screen in from black, only works if faded out.</summary>
    /// <param name="timeInSeconds">number of seconds for the fade in to take. Decimal numbers allowed</param>
    /// <example>&amp;FADE_IN:1</example>
    /// <category>Scene</category>
    private void FADE_IN(float timeInSeconds)
    {
        SceneController.FadeIn(timeInSeconds);
    }

    /// <summary>Pans the camera over a given amount of time to a given position in a straight line. Continues story after starting. Use WAIT to add waiting for completion.</summary>
    /// <param name="duration">number of seconds for the fade in to take. Decimal numbers allowed</param>
    /// <param name="x">x axis position to pan to (0 is the default position)</param>
    /// <param name="y">y axis position to pan to (0 is the default position)</param>
    /// <example>&amp;CAMERA_PAN:2,0,-204</example>
    /// <category>Scene</category>
    private void CAMERA_PAN(float duration, int x, int y)
    {
        SceneController.PanCamera(duration, new Vector2Int(x, y));
        OnActionDone?.Invoke();
    }

    /// <summary>Sets the camera to a given position.</summary>
    /// <param name="x">x axis position to pan to (0 is the default position)</param>
    /// <param name="y">y axis position to pan to (0 is the default position)</param>
    /// <example>&amp;CAMERA_SET:0,-204</example>
    /// <category>Scene</category>
    private void CAMERA_SET(int x, int y)
    {
        SceneController.SetCameraPos(new Vector2Int(x, y));
        OnActionDone?.Invoke();
    }

    /// <summary>Shakes the screen.</summary>
    /// <param name="intensity">Decimal number representing the intensity of the screen shake</param>
    /// <param name="duration">Decimal number representing the duration of the shake in seconds</param>
    /// <param name="isBlocking">(Optional, `false` by default) `true` or `false` for whether the narrative script should continue immediately (`false`) or wait for the shake to finish (`true`)</param>
    /// <example>&amp;SHAKE_SCREEN:1,0.5,true</example>
    /// <category>Scene</category>
    private void SHAKE_SCREEN(float intensity, float duration, bool isBlocking = false)
    {
        SceneController.ShakeScreen(intensity, duration, isBlocking);
    }

    /// <summary>Sets the scene. If an actor was already attached to target scene, it will show up as well.</summary>
    /// <param name="sceneName" validFiles="Assets/Scenes/*.unity">Name of a scene</param>
    /// <example>&amp;SCENE:TMPH_Court</example>
    /// <category>Scene</category>
    private void SCENE(SceneAssetName sceneName)
    {
        SceneController.SetScene(sceneName);
        OnActionDone?.Invoke();
    }
    /// <summary>Shows the given evidence on the screen in the given position.</summary>
    /// <param name="evidence" validFiles="Assets/Resources/Evidence/*.asset">Name of evidence to show</param>
    /// <param name="itemPos">`Left`, `Right` or `Middle`</param>
    /// <example>&amp;SHOW_ITEM:Switch,Left</example>
    /// <category>Scene</category>
    private void SHOW_ITEM(EvidenceAssetName evidence, ItemDisplayPosition itemPos)
    {
        SceneController.ShowItem(evidence, itemPos);
        OnActionDone?.Invoke();
    }

    /// <summary>Hides the item shown when using SHOW_ITEM.</summary>
    /// <example>&amp;HIDE_ITEM</example>
    /// <category>Scene</category>
    private void HIDE_ITEM()
    {
        SceneController.HideItem();
        OnActionDone?.Invoke();
    }

    /// <summary>Plays a fullscreen animation.</summary>
    /// <param name="animationName" validFiles="Assets/Animations/FullscreenAnimations/*.anim">Name of a fullscreen animation to play</param>
    /// <example>&amp;PLAY_ANIMATION:GavelHit</example>
    /// <category>Scene</category>
    private void PLAY_ANIMATION(FullscreenAnimationAssetName animationName)
    {
        SceneController.PlayAnimation(animationName);
    }

    /// <summary>Makes the camera jump to focus on the target sub-position of the currently active scene.</summary>
    /// <param name="slotIndex">Whole number representing the target sub-position of the currently active scene</param>
    /// <example>&amp;JUMP_TO_POSITION:1</example>
    /// <category>Scene</category>
    private void JUMP_TO_POSITION(int slotIndex)
    {
        SceneController.JumpToActorSlot(slotIndex);
        OnActionDone?.Invoke();
    }

    /// <summary>Makes the camera pan to focus on the target sub-position of the currently active scene. Takes the provided amount of time to complete. If you want the system to wait for completion, call WAIT with the appropriate amount of seconds afterwards.</summary>
    /// <param name="slotIndex">Whole number representing the target sub-position of the currently active scene</param>
    /// <param name="panDuration">Decimal number representing the amount of time the pan should take in seconds</param>
    /// <example>&amp;PAN_TO_POSITION:1,1</example>
    /// <category>Scene</category>
    private void PAN_TO_POSITION(int slotIndex, float panDuration)
    {
        SceneController.PanToActorSlot(slotIndex, panDuration);
    }

    /// <summary>Restarts the currently playing script from the beginning.</summary>
    /// <example>&amp;RELOAD_SCENE</example>
    /// <category>Scene</category>
    private void RELOAD_SCENE()
    {
        SceneController.ReloadScene();
    }

    /// <summary>Issues a penalty / deducts one of the attempts available to a player to find the correct piece of evidence or actor during a cross examinaton.</summary>
    /// <example>&amp;ISSUE_PENALTY</example>
    /// <category>Cross Examination</category>
    private void ISSUE_PENALTY()
    {
        PenaltyManager.Decrement();
        OnActionDone?.Invoke();
    }

    /// <summary>Waits for the specified amount of seconds before continuing automatically.</summary>
    /// <param name="seconds">Time in seconds to wait</param>
    /// <example>&amp;WAIT:1</example>
    /// <category>Other</category>
    private void WAIT(float seconds)
    {
        SceneController.Wait(seconds);
    }

    /// <summary>Plays an "Objection!" animation and soundeffect for the specified actor.</summary>
    /// <param name="actorName" validFiles="Assets/Resources/Actors/*.asset">Name of the actor</param>
    /// <example>&amp;OBJECTION:Arin</example>
    /// <category>Dialogue</category>
    private void OBJECTION(ActorAssetName actorName)
    {
        SHOUT(actorName, "Objection", true);
    }

    /// <summary>Plays a "Take that!" animation and soundeffect for the specified actor.</summary>
    /// <param name="actorName" validFiles="Assets/Resources/Actors/*.asset">Name of the actor</param>
    /// <example>&amp;TAKE_THAT:Arin</example>
    /// <category>Dialogue</category>
    private void TAKE_THAT(ActorAssetName actorName)
    {
        SHOUT(actorName, "TakeThat", true);
    }

    /// <summary>Plays a "Hold it!" animation and soundeffect for the specified actor.</summary>
    /// <param name="actorName" validFiles="Assets/Resources/Actors/*.asset">Name of the actor</param>
    /// <example>&amp;HOLD_IT:Arin</example>
    /// <category>Dialogue</category>
    private void HOLD_IT(ActorAssetName actorName)
    {
        SHOUT(actorName, "HoldIt", true);
    }

    /// <summary>Uses the specified actor to play the specified shout.</summary>
    /// <param name="actorName" validFiles="Assets/Resources/Actors/*.asset">Name of the actor to use</param>
    /// <param name="shoutName" validFiles="Assets/Images/Shouts/*.png">Name of the shout to play</param>
    /// <example>&amp;SHOUT:Arin,OBJECTION,false</example>
    /// <category>Dialogue</category>
    private void SHOUT(ActorAssetName actorName, string shoutName, bool allowRandomShouts = false)
    {
        SceneController.Shout(actorName, shoutName, allowRandomShouts);
    }

    /// <summary>Enables the flashing witness testimony sign in the upper left corner of the screen.</summary>
    /// <example>&amp;BEGIN_WITNESS_TESTIMONY</example>
    /// <category>Cross Examination</category>
    private void BEGIN_WITNESS_TESTIMONY()
    {
        SceneController.WitnessTestimonyActive = true;
        OnActionDone?.Invoke();
    }

    /// <summary>Disables the flashing witness testimony sign in the upper left corner of the screen.</summary>
    /// <example>&amp;END_WITNESS_TESTIMONY</example>
    /// <category>Cross Examination</category>
    private void END_WITNESS_TESTIMONY()
    {
        SceneController.WitnessTestimonyActive = false;
        OnActionDone?.Invoke();
    }
    #endregion

    #region ActorController
    /// <summary>Sets the current shown actor on screen to the one provided. Starts it in the normal pose.</summary>
    /// <param name="actorName" validFiles="Assets/Resources/Actors/*.asset">Name of the actor</param>
    /// <example>&amp;ACTOR:Arin</example>
    /// <category>Actor</category>
    private void ACTOR(ActorAssetName actorName)
    {
        ActorController.SetActiveActor(actorName);
        OnActionDone?.Invoke();
    }

    /// <summary>Shows or hides the actor on the screen. Has to be re-done after switching a scene.</summary>
    /// <param name="shouldShow">whether to show (`true`) or not show (`false`) an actor</param>
    /// <example>&amp;SHOW_ACTOR:true</example>
    /// <example>&amp;SHOW_ACTOR:false</example>
    /// <category>Actor</category>
    private void SHOW_ACTOR(bool shouldShow)
    {
        if (shouldShow)
        {
            SceneController.ShowActor();
        }
        else
        {
            SceneController.HideActor();
        }

        OnActionDone?.Invoke();
    }

    /// <summary>Makes the next non-action line spoken by the provided actor. If the speaking actor matches the actor on screen, it makes their mouth move when speaking.</summary>
    /// <param name="actorName" validFiles="Assets/Resources/Actors/*.asset">Name of the actor</param>
    /// <example>&amp;SPEAK:Arin</example>
    /// <category>Dialogue</category>
    private void SPEAK(ActorAssetName actorName)
    {
        SetSpeaker(actorName, SpeakingType.Speaking);
        OnActionDone?.Invoke();
    }

    /// <summary>Makes the next non-action line spoken by the provided actor. Doesn't make the actor's mouth.</summary>
    /// <param name="actorName" validFiles="Assets/Resources/Actors/*.asset">Name of the actor</param>
    /// <example>&amp;THINK:Arin</example>
    /// <category>Dialogue</category>
    private void THINK(ActorAssetName actorName)
    {
        SetSpeaker(actorName, SpeakingType.Thinking);
        OnActionDone?.Invoke();
    }

    /// <summary>Makes the next non-action line spoken by the provided actor but hides the name.</summary>
    /// <param name="actorName" validFiles="Assets/Resources/Actors/*.asset">Name of the actor</param>
    /// <example>&amp;SPEAK_UNKNOWN:Arin</example>
    /// <category>Dialogue</category>
    private void SPEAK_UNKNOWN(ActorAssetName actorName)
    {
        SetSpeaker(actorName, SpeakingType.SpeakingWithUnknownName);
        OnActionDone?.Invoke();
    }

    /// <summary>Makes the next non-action line spoken by a "narrator" actor.</summary>
    /// <example>&amp;NARRATE:Arin</example>
    /// <category>Dialogue</category>
    private void NARRATE()
    {
        ActorController.SetActiveSpeakerToNarrator();
        ActorController.SetSpeakingType(SpeakingType.Speaking);
        OnActionDone?.Invoke();
    }

    private void SetSpeaker(ActorAssetName actorName, SpeakingType speakingType)
    {
        ActorController.SetActiveSpeaker(actorName, speakingType);
        ActorController.SetSpeakingType(speakingType);
    }

    /// <summary>Makes the currently shown actor switch to target pose. Plays any animation associated with target pose / emotion, but doesn't wait until it is finished before continuing.</summary>
    /// <param name="poseName" validFiles="Assets/Animations/{ActorAssetName}/*.anim">Poses defined per Actor</param>
    /// <param name="optional_targetActor" validFiles="Assets/Resources/Actors/*.asset">(optional) Name of the actor</param>
    /// <example>&amp;SET_POSE:Normal</example>
    /// <category>Actor</category>
    private void SET_POSE(ActorPoseAssetName poseName, ActorAssetName optional_targetActor = null)
    {
        if (optional_targetActor == null)
        {
            ActorController.SetPose(poseName);
        }
        else
        {
            ActorController.SetPose(poseName, optional_targetActor);
        }
        OnActionDone?.Invoke();
    }

    /// <summary>Makes the currently shown actor perform target emotion (fancy word animation on an actor). Practically does the same as SET_POSE, but waits for the emotion to complete. Doesn't work on all poses, possible ones are flagged.</summary>
    /// <param name="poseName" validFiles="Assets/Animations/{ActorAssetName}/*.anim">Poses defined per Actor</param>
    /// <param name="optional_targetActor" validFiles="Assets/Resources/Actors/*.asset">(optional) Name of the actor</param>
    /// <example>&amp;PLAY_EMOTION:Nodding</example>
    /// <category>Actor</category>
    private void PLAY_EMOTION(ActorPoseAssetName poseName, ActorAssetName? optional_targetActor = null)
    {
        if (optional_targetActor == null)
        {
            ActorController.PlayEmotion(poseName);
        }
        else
        {
            ActorController.PlayEmotion(poseName, optional_targetActor);
        }
    }

    /// <summary>Sets the target sub-position of the current bg-scene to have the target actor.</summary>
    /// <param name="oneBasedSlotIndex">Whole number representing the target sub-position of the currently active scene</param>
    /// <param name="actorName" validFiles="Assets/Resources/Actors/*.asset">Name of an actor</param>
    /// <example>&amp;SET_ACTOR_POSITION:1,Arin</example>
    /// <category>Actor</category>
    private void SET_ACTOR_POSITION(int oneBasedSlotIndex, ActorAssetName actorName)
    {
        ActorController.AssignActorToSlot(actorName, oneBasedSlotIndex);
        OnActionDone?.Invoke();
    }

    /// <summary>Unlocks a new chapter inside the chapter select. **(This is persistent, even when the game is restarted!)**</summary>
    /// <param name="chapter">Name of the chapter to unlock</param>
    /// <example>&amp;UNLOCK_CHAPTER:CHAPTER_2</example>
    /// <example>&amp;UNLOCK_CHAPTER:BONUS_CHAPTER_2</example>
    /// <category>Progression</category>
    private void UNLOCK_CHAPTER(SaveData.Progression.Chapters chapter)
    {
        PlayerPrefsProxy.UpdateCurrentSaveData((ref SaveData data) => {
            data.GameProgression.UnlockedChapters |= chapter;
        });
        OnActionDone?.Invoke();
    }
    #endregion
    #region DialogueController
    /// <summary>Changes the game mode. (This decides how the user is able to progress with the story.)</summary>
    /// <param name="mode">Name of game mode to put the player in</param>
    /// <example>&amp;MODE:CrossExamination</example>
    /// <category>Gameplay</category>
    private void MODE(GameMode mode)
    {
        DialogueController.GameMode = mode;
        switch (mode)
        {
            case GameMode.Dialogue:
                PenaltyManager.OnCrossExaminationEnd();
                break;
            case GameMode.CrossExamination:
                PenaltyManager.OnCrossExaminationStart();
                break;
            default:
                throw new NotSupportedException($"Switching to game mode '{mode}' is not supported");
        }

        OnActionDone?.Invoke();
    }

    /// <summary>
    /// Resets the number of penalties the player has left.
    /// </summary>
    /// <example>&amp;RESET_PENALTIES</example>
    /// <category>Gameplay</category>
    private void RESET_PENALTIES()
    {
        PenaltyManager.ResetPenalties();
        OnActionDone?.Invoke();
    }
    
    #endregion
#pragma warning restore IDE0051 // Remove unused private members
// ReSharper restore UnusedMember.Local
    // ReSharper restore InconsistentNaming
}
