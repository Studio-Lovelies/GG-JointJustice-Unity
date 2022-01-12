using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionDecoder : ActionDecoderBase
{
    public event Action OnActionDone;
    public IActorController ActorController { get; set; }
    public ISceneController SceneController { get; set; }
    public IAudioController AudioController { get; set; }
    public IEvidenceController EvidenceController { get; set; }
    public IAppearingDialogueController AppearingDialogueController { get; set; }

    // ReSharper disable InconsistentNaming
    // ReSharper disable UnusedMember.Local
#pragma warning disable IDE0051 // Remove unused private members
    #region AppearingDialogueController
    private void DIALOGUE_SPEED(float characterDelay)
    {
        AppearingDialogueController.CharacterDelay = characterDelay;
        OnActionDone?.Invoke();
    }

    private void PUNCTUATION_SPEED(float seconds)
    {
        AppearingDialogueController.DefaultPunctuationDelay = seconds;
        OnActionDone?.Invoke();
    }

    private void DISABLE_SKIPPING(bool value)
    {
        AppearingDialogueController.SkippingDisabled = value;
        OnActionDone?.Invoke();
    }

    private void CONTINUE_DIALOGUE()
    {
        AppearingDialogueController.ContinueDialogue = true;
        OnActionDone?.Invoke();
    }

    private void AUTO_SKIP(bool value)
    {
        AppearingDialogueController.AutoSkip = value;
        OnActionDone?.Invoke();
    }

    private void APPEAR_INSTANTLY()
    {
        AppearingDialogueController.AppearInstantly = true;
        OnActionDone?.Invoke();
    }

    private void HIDE_TEXTBOX()
    {
        AppearingDialogueController.TextBoxHidden = true;
        OnActionDone?.Invoke();
    }
    #endregion

    #region EvidenceController
    protected override void ADD_EVIDENCE(AssetName evidenceName)
    {
        EvidenceController.AddEvidence(evidenceName);
        OnActionDone?.Invoke();
    }

    private void REMOVE_EVIDENCE(AssetName evidence)
    {
        EvidenceController.RemoveEvidence(evidence);
        OnActionDone?.Invoke();
    }

    protected override void ADD_RECORD(AssetName actorName)
    {
        EvidenceController.AddToCourtRecord(actorName);
        OnActionDone?.Invoke();
    }
    
    private void PRESENT_EVIDENCE()
    {
        EvidenceController.RequirePresentEvidence();
    }
    
    private void SUBSTITUTE_EVIDENCE(AssetName evidence)
    {
        EvidenceController.SubstituteEvidenceWithAlt(evidence);
        OnActionDone?.Invoke();
    }
    #endregion

    #region AudioController

    protected override void PLAY_SFX(AssetName sfx)
    {
        AudioController.PlaySfx(sfx);
        OnActionDone?.Invoke();
    }

    protected override void PLAY_SONG(AssetName songName)
    {
        AudioController.PlaySong(songName);
        OnActionDone?.Invoke();
    }

    private void STOP_SONG()
    {
        AudioController.StopSong();
        OnActionDone?.Invoke();
    }
    #endregion

    #region SceneController
    private void FADE_IN(float timeInSeconds)
    {
        SceneController.FadeIn(timeInSeconds);
    }

    private void FADE_OUT(float timeInSeconds)
    {
        SceneController.FadeOut(timeInSeconds);
    }

    private void SHAKE_SCREEN(float intensity, float duration, bool isBlocking = false)
    {
        SceneController.ShakeScreen(intensity, duration, isBlocking);
    }

    protected override void SCENE(AssetName sceneName)
    {
        SceneController.SetScene(sceneName);
        OnActionDone?.Invoke();
    }

    private void CAMERA_SET(int x, int y)
    {
        SceneController.SetCameraPos(new Vector2Int(x, y));
        OnActionDone?.Invoke();
    }

    private void CAMERA_PAN(float duration, int x, int y)
    {
        SceneController.PanCamera(duration, new Vector2Int(x, y));
        OnActionDone?.Invoke();
    }

    protected override void SHOW_ITEM(AssetName itemName, ItemDisplayPosition itemPos)
    {
        SceneController.ShowItem(itemName, itemPos);
        OnActionDone?.Invoke();
    }

    private void HIDE_ITEM()
    {
        SceneController.HideItem();
        OnActionDone?.Invoke();
    }

    private void WAIT(float seconds)
    {
        SceneController.Wait(seconds);
    }

    private void PLAY_ANIMATION(AssetName animationName)
    {
        SceneController.PlayAnimation(animationName);
    }

    private void JUMP_TO_POSITION(int slotIndex)
    {
        SceneController.JumpToActorSlot(slotIndex);
        OnActionDone?.Invoke();
    }

    private void PAN_TO_POSITION(int slotIndex, float panDuration)
    {
        SceneController.PanToActorSlot(slotIndex, panDuration);
    }

    private void ISSUE_PENALTY()
    {
        SceneController.IssuePenalty();
        OnActionDone?.Invoke();
    }

    private void RELOAD_SCENE()
    {
        SceneController.ReloadScene();
    }

    private void BEGIN_WITNESS_TESTIMONY()
    {
        SceneController.WitnessTestimonyActive = true;
        OnActionDone?.Invoke();
    }

    private void END_WITNESS_TESTIMONY()
    {
        SceneController.WitnessTestimonyActive = false;
        OnActionDone?.Invoke();
    }
    
    #endregion

    #region ActorController

    protected override void ACTOR(AssetName actorName)
    {
        ActorController.SetActiveActor(actorName);
        OnActionDone?.Invoke();
    }

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

    protected override void SPEAK(AssetName actorName)
    {
        SetSpeaker(actorName, SpeakingType.Speaking);
    }

    protected override void SPEAK_UNKNOWN(AssetName actor)
    {
        SetSpeaker(actor, SpeakingType.SpeakingWithUnknownName);
    }
    
    private void NARRATE()
    {
        ActorController.SetActiveSpeakerToNarrator();
        ActorController.SetSpeakingType(SpeakingType.Speaking);
        OnActionDone?.Invoke();
    }

    protected override void THINK(AssetName actorName)
    {
        SetSpeaker(actorName, SpeakingType.Thinking);
    }

    private void SetSpeaker(string actor, SpeakingType speakingType)
    {
        ActorController.SetActiveSpeaker(actor, speakingType);
        ActorController.SetSpeakingType(speakingType);
        OnActionDone?.Invoke();
    }

    private void SET_POSE(AssetName poseName, string optional_targetActor = null)
    {
        if (optional_targetActor == null)
        {
            ActorController.SetPose(poseName);
            OnActionDone?.Invoke();
        }
        else
        {
            ActorController.SetPose(poseName, optional_targetActor);
            OnActionDone?.Invoke();
        }
    }

    private void PLAY_EMOTION(AssetName poseName, AssetName? optional_targetActor = null)
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

    protected override void SET_ACTOR_POSITION(int oneBasedSlotIndex, AssetName actorName)
    {
        ActorController.AssignActorToSlot(actorName, oneBasedSlotIndex);
        OnActionDone?.Invoke();
    }
    #endregion
#pragma warning restore IDE0051 // Remove unused private members
    // ReSharper restore UnusedMember.Local
    // ReSharper restore InconsistentNaming
}
