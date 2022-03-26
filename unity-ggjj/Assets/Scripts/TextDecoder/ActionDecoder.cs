using System;
using SaveFiles;
using UnityEngine;

public class ActionDecoder : ActionDecoderBase
{
    public event Action OnActionDone;
    public INarrativeGameState NarrativeGameState { get; set; }

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
        NarrativeGameState.AppearingDialogueController.CharacterDelay = characterDelay;
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
        NarrativeGameState.AppearingDialogueController.DefaultPunctuationDelay = seconds;
        OnActionDone?.Invoke();
    }

    /// <summary>Starts or stops autoskipping of dialogue, where it automatically continues after it is done.</summary>
    /// <param name="value">Set to either `true` or `false` to enable or disable automatic dialogue skipping respectively.</param>
    /// <example>&amp;AUTO_SKIP:true</example>
    /// <example>&amp;AUTO_SKIP:false</example>
    /// <category>Dialogue</category>
    private void AUTO_SKIP(bool value)
    {
        NarrativeGameState.AppearingDialogueController.AutoSkip = value;
        OnActionDone?.Invoke();
    }

    /// <summary>Disables or enables text speedup. Enabled by default.</summary>
    /// <param name="value">Set to either `true` or `false` to not speedup or speedup text respectively.</param>
    /// <example>&amp;DISABLE_SKIPPING:true</example>
    /// <example>&amp;DISABLE_SKIPPING:false</example>
    /// <category>Dialogue</category>
    private void DISABLE_SKIPPING(bool value)
    {
        NarrativeGameState.AppearingDialogueController.SkippingDisabled = value;
        OnActionDone?.Invoke();
    }

    /// <summary>Makes the next dialogue add to the current one instead of replacing it.</summary>
    /// <example>&amp;CONTINUE_DIALOGUE</example>
    /// <category>Dialogue</category>
    private void CONTINUE_DIALOGUE()
    {
        NarrativeGameState.AppearingDialogueController.ContinueDialogue = true;
        OnActionDone?.Invoke();
    }

    /// <summary>Makes the next line of dialogue appear all at once, instead of character by character.</summary>
    /// <category>Dialogue</category>
    /// <example>&amp;APPEAR_INSTANTLY</example>
    private void APPEAR_INSTANTLY()
    {
        NarrativeGameState.AppearingDialogueController.AppearInstantly = true;
        OnActionDone?.Invoke();
    }

    /// <summary>Hides the dialogue textbox until the next line of dialogue.</summary>
    /// <category>Dialogue</category>
    /// <example>&amp;HIDE_TEXTBOX</example>
    private void HIDE_TEXTBOX()
    {
        NarrativeGameState.AppearingDialogueController.TextBoxHidden = true;
        OnActionDone?.Invoke();
    }
    #endregion

    #region EvidenceController
    /// <summary>Adds the provided evidence to the court record.</summary>
    /// <param name="evidence" validFiles="Assets/Resources/Evidence/*.asset">Name of evidence to add</param>
    /// <example>&amp;ADD_EVIDENCE:Bent_Coins</example>
    /// <category>Evidence</category>
    protected override void ADD_EVIDENCE(EvidenceAssetName evidence)
    {
        NarrativeGameState.EvidenceController.AddEvidence(NarrativeGameState.ObjectStorage.GetObject<Evidence>(evidence));
        OnActionDone?.Invoke();
    }

    /// <summary>Removes the provided evidence from the court record.</summary>
    /// <param name="evidence" validFiles="Assets/Resources/Evidence/*.asset">Name of evidence to remove</param>
    /// <example>&amp;REMOVE_EVIDENCE:Bent_Coins</example>
    /// <category>Evidence</category>
    private void REMOVE_EVIDENCE(EvidenceAssetName evidence)
    {
        NarrativeGameState.EvidenceController.RemoveEvidence(NarrativeGameState.ObjectStorage.GetObject<Evidence>(evidence));
        OnActionDone?.Invoke();
    }

    /// <summary>Adds the provided actor to the court record.</summary>
    /// <param name="actorName" validFiles="Assets/Resources/Actors/*.asset">Name of the actor to add to the court record</param>
    /// <example>&amp;ADD_RECORD:Jory</example>
    /// <category>Evidence</category>
    protected override void ADD_RECORD(ActorAssetName actorName)
    {
        NarrativeGameState.EvidenceController.AddRecord(NarrativeGameState.ObjectStorage.GetObject<ActorData>(actorName));
        OnActionDone?.Invoke();
    }

    /// <summary>Forces the evidence menu open and doesn't continue the story until the player presents evidence.</summary>
    /// <example>&amp;PRESENT_EVIDENCE</example>
    /// <category>Evidence</category>
    private void PRESENT_EVIDENCE()
    {
        NarrativeGameState.EvidenceController.RequirePresentEvidence();
        NarrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.GameMode = GameMode.CrossExamination;
    }

    /// <summary>Substitutes the provided evidence for their substitute.</summary>
    /// <param name="initialEvidenceName" validFiles="Assets/Resources/Evidence/*.asset">Name of evidence to replace with the substitute</param>
    /// <param name="substituteEvidenceName" validFiles="Assets/Resources/Evidence/*.asset">Name of the substitute evidence</param>
    /// <example>&amp;SUBSTITUTE_EVIDENCE:Plumber_Invoice,Bent_Coins</example>
    /// <category>Evidence</category>
    private void SUBSTITUTE_EVIDENCE(EvidenceAssetName initialEvidenceName, EvidenceAssetName substituteEvidenceName)
    {
        NarrativeGameState.EvidenceController.SubstituteEvidence(NarrativeGameState.ObjectStorage.GetObject<Evidence>(initialEvidenceName), NarrativeGameState.ObjectStorage.GetObject<Evidence>(substituteEvidenceName));
        OnActionDone?.Invoke();
    }
    #endregion

    #region AudioController
    /// <summary>Plays provided SFX.</summary>
    /// <param name="sfx" validFiles="Assets/Resources/Audio/SFX/*.wav">Filename of a sound effect</param>
    /// <example>&amp;PLAY_SFX:EvidenceShoop</example>
    /// <category>Audio</category>
    protected override void PLAY_SFX(SfxAssetName sfx)
    {
        NarrativeGameState.AudioController.PlaySfx(NarrativeGameState.ObjectStorage.GetObject<AudioClip>(sfx));
        OnActionDone?.Invoke();
    }

    /// <summary>Plays the provided song. Stops the current one. Loops infinitely.</summary>
    /// <param name="songName" validFiles="Assets/Resources/Audio/Music/*.mp3">Filename of a song</param>
    /// <param name="optional_transitionTime">(Optional) The time taken to transition between songs</param>
    /// <example>&amp;PLAY_SONG:TurnaboutGrumpsters</example>
    /// <category>Audio</category>
    protected override void PLAY_SONG(SongAssetName songName, float optional_transitionTime = 0)
    {
        NarrativeGameState.AudioController.PlaySong(NarrativeGameState.ObjectStorage.GetObject<AudioClip>(songName), optional_transitionTime);
        OnActionDone?.Invoke();
    }

    /// <summary>If music is currently playing, stop it.</summary>
    /// <example>&amp;STOP_SONG</example>
    /// <category>Audio</category>
    private void STOP_SONG()
    {
        NarrativeGameState.AudioController.StopSong();
        OnActionDone?.Invoke();
    }

    /// <summary>
    /// Fade out the currently playing song over a given time
    /// </summary>
    /// <param name="time">The time taken to fade out</param>
    /// <example>&amp;FADE_OUT_SONG:2</example>
    /// <category>Audio</category>
    private void FADE_OUT_SONG(float time)
    {
        NarrativeGameState.AudioController.FadeOutSong(time);
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
        NarrativeGameState.SceneController.FadeOut(timeInSeconds);
    }

    /// <summary>Fades the screen in from black, only works if faded out.</summary>
    /// <param name="timeInSeconds">number of seconds for the fade in to take. Decimal numbers allowed</param>
    /// <example>&amp;FADE_IN:1</example>
    /// <category>Scene</category>
    private void FADE_IN(float timeInSeconds)
    {
        NarrativeGameState.SceneController.FadeIn(timeInSeconds);
    }

    /// <summary>Pans the camera over a given amount of time to a given position in a straight line. Continues story after starting. Use WAIT to add waiting for completion.</summary>
    /// <param name="duration">number of seconds for the fade in to take. Decimal numbers allowed</param>
    /// <param name="x">x axis position to pan to (0 is the default position)</param>
    /// <param name="y">y axis position to pan to (0 is the default position)</param>
    /// <example>&amp;CAMERA_PAN:2,0,-204</example>
    /// <category>Scene</category>
    private void CAMERA_PAN(float duration, int x, int y)
    {
        NarrativeGameState.SceneController.PanCamera(duration, new Vector2Int(x, y));
        OnActionDone?.Invoke();
    }

    /// <summary>Sets the camera to a given position.</summary>
    /// <param name="x">x axis position to pan to (0 is the default position)</param>
    /// <param name="y">y axis position to pan to (0 is the default position)</param>
    /// <example>&amp;CAMERA_SET:0,-204</example>
    /// <category>Scene</category>
    private void CAMERA_SET(int x, int y)
    {
        NarrativeGameState.SceneController.SetCameraPos(new Vector2Int(x, y));
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
        NarrativeGameState.SceneController.ShakeScreen(intensity, duration, isBlocking);
    }

    /// <summary>Sets the scene. If an actor was already attached to target scene, it will show up as well.</summary>
    /// <param name="sceneName" validFiles="Assets/Resources/BGScenes/*.prefab">Name of a scene</param>
    /// <example>&amp;SCENE:TMPH_Court</example>
    /// <category>Scene</category>
    protected override void SCENE(SceneAssetName sceneName)
    {
        NarrativeGameState.SceneController.SetScene(sceneName);
        OnActionDone?.Invoke();
    }
    
    /// <summary>Shows the given evidence on the screen in the given position.</summary>
    /// <param name="item" validFiles="Assets/Resources/Evidence/*.asset">Name of item to show</param>
    /// <param name="itemPos">`Left`, `Right` or `Middle`</param>
    /// <example>&amp;SHOW_ITEM:Switch,Left</example>
    /// <category>Scene</category>
    protected override void SHOW_ITEM(EvidenceAssetName item, ItemDisplayPosition itemPos)
    {
        NarrativeGameState.SceneController.ShowItem(NarrativeGameState.ObjectStorage.GetObject<ICourtRecordObject>(item), itemPos);
        OnActionDone?.Invoke();
    }

    /// <summary>Hides the item shown when using SHOW_ITEM.</summary>
    /// <example>&amp;HIDE_ITEM</example>
    /// <category>Scene</category>
    private void HIDE_ITEM()
    {
        NarrativeGameState.SceneController.HideItem();
        OnActionDone?.Invoke();
    }

    /// <summary>Plays a fullscreen animation.</summary>
    /// <param name="animationName" validFiles="Assets/Animations/FullscreenAnimations/*.anim">Name of a fullscreen animation to play</param>
    /// <example>&amp;PLAY_ANIMATION:GavelHit</example>
    /// <category>Scene</category>
    private void PLAY_ANIMATION(FullscreenAnimationAssetName animationName)
    {
        NarrativeGameState.SceneController.PlayAnimation(animationName);
    }

    /// <summary>Makes the camera jump to focus on the target sub-position of the currently active scene.</summary>
    /// <param name="slotIndex">Whole number representing the target sub-position of the currently active scene</param>
    /// <example>&amp;JUMP_TO_POSITION:1</example>
    /// <category>Scene</category>
    private void JUMP_TO_POSITION(int slotIndex)
    {
        NarrativeGameState.SceneController.JumpToActorSlot(slotIndex);
        OnActionDone?.Invoke();
    }

    /// <summary>Makes the camera pan to focus on the target sub-position of the currently active scene. Takes the provided amount of time to complete. If you want the system to wait for completion, call WAIT with the appropriate amount of seconds afterwards.</summary>
    /// <param name="slotIndex">Whole number representing the target sub-position of the currently active scene</param>
    /// <param name="panDuration">Decimal number representing the amount of time the pan should take in seconds</param>
    /// <example>&amp;PAN_TO_POSITION:1,1</example>
    /// <category>Scene</category>
    private void PAN_TO_POSITION(int slotIndex, float panDuration)
    {
        NarrativeGameState.SceneController.PanToActorSlot(slotIndex, panDuration);
    }

    /// <summary>Restarts the currently playing script from the beginning.</summary>
    /// <example>&amp;RELOAD_SCENE</example>
    /// <category>Scene</category>
    private void RELOAD_SCENE()
    {
        NarrativeGameState.SceneController.ReloadScene();
    }

    /// <summary>Issues a penalty / deducts one of the attempts available to a player to find the correct piece of evidence or actor during a cross examinaton.</summary>
    /// <example>&amp;ISSUE_PENALTY</example>
    /// <category>Cross Examination</category>
    private void ISSUE_PENALTY()
    {
        NarrativeGameState.PenaltyManager.Decrement();
        OnActionDone?.Invoke();
    }

    /// <summary>Waits for the specified amount of seconds before continuing automatically.</summary>
    /// <param name="seconds">Time in seconds to wait</param>
    /// <example>&amp;WAIT:1</example>
    /// <category>Other</category>
    private void WAIT(float seconds)
    {
        NarrativeGameState.SceneController.Wait(seconds);
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
        NarrativeGameState.SceneController.Shout(actorName, shoutName, allowRandomShouts);
    }

    /// <summary>Enables the flashing witness testimony sign in the upper left corner of the screen.</summary>
    /// <example>&amp;BEGIN_WITNESS_TESTIMONY</example>
    /// <category>Cross Examination</category>
    private void BEGIN_WITNESS_TESTIMONY()
    {
        NarrativeGameState.SceneController.WitnessTestimonyActive = true;
        OnActionDone?.Invoke();
    }

    /// <summary>Disables the flashing witness testimony sign in the upper left corner of the screen.</summary>
    /// <example>&amp;END_WITNESS_TESTIMONY</example>
    /// <category>Cross Examination</category>
    private void END_WITNESS_TESTIMONY()
    {
        NarrativeGameState.SceneController.WitnessTestimonyActive = false;
        OnActionDone?.Invoke();
    }
    #endregion

    #region ActorController
    /// <summary>Sets the current shown actor on screen to the one provided. Starts it in the normal pose.</summary>
    /// <param name="actorName" validFiles="Assets/Resources/Actors/*.asset">Name of the actor</param>
    /// <example>&amp;ACTOR:Arin</example>
    /// <category>Actor</category>
    protected override void ACTOR(ActorAssetName actorName)
    {
        NarrativeGameState.ActorController.SetActiveActor(actorName);
        OnActionDone?.Invoke();
    }

    /// <summary>Shows the current active actor, or a specified actor in the scene</summary>
    /// <param name="optional_actorName" validFiles="Assets/Resources/Actors/*.asset">(Optional) Name of the actor to show</param>
    /// <example>&amp;SHOW_ACTOR</example>
    /// <example>&amp;SHOW_ACTOR:Arin</example>
    /// <category>Actor</category>
    private void SHOW_ACTOR(ActorAssetName optional_actorName = null)
    {
        NarrativeGameState.ActorController.SetVisibility(true, optional_actorName);
        OnActionDone?.Invoke();
    }

    /// <summary>Hides the current active actor, or a specified actor in the scene</summary>
    /// <param name="optional_actorName" validFiles="Assets/Resources/Actors/*.asset">(Optional) Name of the actor to hide</param>
    /// <example>&amp;HIDE_ACTOR</example>
    /// <example>&amp;HIDE_ACTOR:Arin</example>
    /// <category>Actor</category>
    private void HIDE_ACTOR(ActorAssetName optional_actorName = null)
    {
        NarrativeGameState.ActorController.SetVisibility(false, optional_actorName);
        OnActionDone?.Invoke();
    }

    /// <summary>Makes the next non-action line spoken by the provided actor. If the speaking actor matches the actor on screen, it makes their mouth move when speaking.</summary>
    /// <param name="actorName" validFiles="Assets/Resources/Actors/*.asset">Name of the actor</param>
    /// <example>&amp;SPEAK:Arin</example>
    /// <category>Dialogue</category>
    protected override void SPEAK(ActorAssetName actorName)
    {
        NarrativeGameState.ActorController.SetActiveSpeaker(actorName, SpeakingType.Speaking);
        OnActionDone?.Invoke();
    }

    /// <summary>Makes the next non-action line spoken by the provided actor. Doesn't make the actor's mouth.</summary>
    /// <param name="actorName" validFiles="Assets/Resources/Actors/*.asset">Name of the actor</param>
    /// <example>&amp;THINK:Arin</example>
    /// <category>Dialogue</category>
    protected override void THINK(ActorAssetName actorName)
    {
        NarrativeGameState.ActorController.SetActiveSpeaker(actorName, SpeakingType.Thinking);
        OnActionDone?.Invoke();
    }

    /// <summary>Makes the next non-action line spoken by the provided actor but hides the name.</summary>
    /// <param name="actorName" validFiles="Assets/Resources/Actors/*.asset">Name of the actor</param>
    /// <example>&amp;SPEAK_UNKNOWN:Arin</example>
    /// <category>Dialogue</category>
    protected override void SPEAK_UNKNOWN(ActorAssetName actorName)
    {
        NarrativeGameState.ActorController.SetActiveSpeaker(actorName, SpeakingType.SpeakingWithUnknownName);
        OnActionDone?.Invoke();
    }

    /// <summary>Makes the next non-action line spoken by a "narrator" actor.</summary>
    /// <example>&amp;NARRATE:Arin</example>
    /// <category>Dialogue</category>
    private void NARRATE()
    {
        NarrativeGameState.ActorController.SetActiveSpeakerToNarrator();
        OnActionDone?.Invoke();
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
            NarrativeGameState.ActorController.SetPose(poseName);
        }
        else
        {
            NarrativeGameState.ActorController.SetPose(poseName, optional_targetActor);
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
            NarrativeGameState.ActorController.PlayEmotion(poseName);
        }
        else
        {
            NarrativeGameState.ActorController.PlayEmotion(poseName, optional_targetActor);
        }
    }

    /// <summary>Sets the target sub-position of the current bg-scene to have the target actor.</summary>
    /// <param name="oneBasedSlotIndex">Whole number representing the target sub-position of the currently active scene</param>
    /// <param name="actorName" validFiles="Assets/Resources/Actors/*.asset">Name of an actor</param>
    /// <example>&amp;SET_ACTOR_POSITION:1,Arin</example>
    /// <category>Actor</category>
    protected override void SET_ACTOR_POSITION(int oneBasedSlotIndex, ActorAssetName actorName)
    {
        NarrativeGameState.ActorController.AssignActorToSlot(actorName, oneBasedSlotIndex);
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

    #region Gameplay
    /// <summary>Changes the game mode. (This decides how the user is able to progress with the story.)</summary>
    /// <param name="mode">Name of game mode to put the player in</param>
    /// <example>&amp;MODE:CrossExamination</example>
    /// <category>Gameplay</category>
    private void MODE(GameMode mode)
    {
        NarrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.GameMode = mode;
        switch (mode)
        {
            case GameMode.Dialogue:
                NarrativeGameState.PenaltyManager.OnCrossExaminationEnd();
                break;
            case GameMode.CrossExamination:
                NarrativeGameState.PenaltyManager.OnCrossExaminationStart();
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
        NarrativeGameState.PenaltyManager.ResetPenalties();
        OnActionDone?.Invoke();
    }

    /// <summary>
    /// Loads a narrative script, ending the current narrative script
    /// and continuing the beginning of the loaded script
    /// </summary>
    /// <param name="narrativeScriptName" validFiles="Assets/Resources/InkDialogueScripts/*.ink">The name of the narrative script to load</param>
    /// <example>&amp;LOAD_SCRIPT:Case_1_Part_1</example>
    /// <category>Script Loading</category>
    private void LOAD_SCRIPT(NarrativeScriptAssetName narrativeScriptName)
    {
        NarrativeGameState.NarrativeScriptPlayerComponent.LoadScript(narrativeScriptName);
        OnActionDone?.Invoke();
    }

    /// <summary>
    /// Sets the game over narrative script for the currently playing narrative script
    /// </summary>
    /// <param name="gameOverScriptName" validFiles="Assets/Resources/InkDialogueScripts/Failures/*.ink">The name of the game over script</param>
    /// <example>&amp;SET_GAME_OVER_SCRIPT:TMPH_GameOver</example>
    /// <category>Script Loading</category>
    private void SET_GAME_OVER_SCRIPT(GameOverScriptAssetName gameOverScriptName)
    {
        NarrativeGameState.NarrativeScriptStorage.SetGameOverScript(gameOverScriptName);
        OnActionDone?.Invoke();
    }

    /// <summary>
    /// Adds a failure script for the currently playing narrative script
    /// </summary>
    /// <param name="failureScriptName" validFiles="Assets/Resources/InkDialogueScripts/Failures/*.ink">The name of the failure script to add</param>
    /// <example>&amp;ADD_FAILURE_SCRIPT:TMPH_FAIL_1</example>
    /// <category>Script Loading</category>
    private void ADD_FAILURE_SCRIPT(FailureScriptAssetName failureScriptName)
    {
        NarrativeGameState.NarrativeScriptStorage.AddFailureScript(failureScriptName);
        OnActionDone?.Invoke();
    }
    #endregion
#pragma warning restore IDE0051 // Remove unused private members
// ReSharper restore UnusedMember.Local
    // ReSharper restore InconsistentNaming
}
