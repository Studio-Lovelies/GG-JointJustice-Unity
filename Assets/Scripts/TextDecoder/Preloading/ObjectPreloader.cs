using System;
using UnityEngine;

public class ObjectPreloader : IActorController, ISceneController, IEvidenceController, IAudioController, IAppearingDialogueController
{
    private const string ACTOR_PATH = "Actors/";
    private const string EVIDENCE_PATH = "Evidence/";
    private const string MUSIC_PATH = "Audio/Music/";
    private const string SFX_PATH = "Audio/SFX/";
    private const string BGSCENE_PATH = "BGScenes/";

    private IObjectLoader _objectLoader = new ResourceLoader();
    private ObjectStorage _objectStorage;

    public float CharacterDelay { get; set; }
    public float DefaultPunctuationDelay { get; set; }
    public bool SkippingDisabled { get; set; }
    public bool ContinueDialogue { get; set; }
    public bool AutoSkip { get; set; }
    public bool AppearInstantly { get; set; }
    public bool TextBoxHidden { get; set; }

    public ObjectPreloader(ObjectStorage objectStorage)
    {
        _objectStorage = objectStorage;
    }
    
    public void SetActiveActor(string actor)
    {
        LoadActor(actor);
    }

    public void SetActiveSpeaker(string actor)
    {
        LoadActor(actor);
    }

    public void SetPose(string pose, string actorName = null)
    {
    }

    public void PlayEmotion(string emotion, string actorName = null)
    {
    }

    public void StartTalking()
    {
    }

    public void StopTalking()
    {
    }

    public void OnAnimationDone()
    {
    }

    public void SetSpeakingType(SpeakingType speakingType)
    {
    }

    public void AssignActorToSlot(string actor, int oneBasedSlotIndex)
    {
        LoadActor(actor);
    }

    public void FadeIn(float seconds)
    {
    }

    public void FadeOut(float seconds)
    {
    }

    public void ShakeScreen(float intensity, float duration, bool isBlocking)
    {
    }

    public void SetScene(string background)
    {
        LoadScene(background);
    }

    public void SetCameraPos(Vector2Int position)
    {
    }

    public void PanCamera(float seconds, Vector2Int finalPosition)
    {
    }

    public void PanToActorSlot(int oneBasedSlotIndex, float seconds)
    {
    }

    public void JumpToActorSlot(int oneBasedSlotIndex)
    {
    }

    public void ShowItem(string item, ItemDisplayPosition position)
    {
        LoadEvidence(item);
    }

    public void ShowActor()
    {
    }

    public void HideActor()
    {
    }

    public void Wait(float seconds)
    {
    }

    public void HideItem()
    {
    }

    public void PlayAnimation(string animationName)
    {
    }

    public void IssuePenalty()
    {
    }

    public void ReloadScene()
    {
    }

    public void AddEvidence(string evidenceName)
    {
        LoadEvidence(evidenceName);
    }

    public void RemoveEvidence(string evidenceName)
    {
    }

    public void AddToCourtRecord(string actorName)
    {
        LoadActor(actorName);
    }

    public void RequirePresentEvidence()
    {
    }

    public void SubstituteEvidenceWithAlt(string evidenceName)
    {
    }

    public void OnPresentEvidence(ICourtRecordObject evidence)
    {
    }
    
    public void PlaySfx(string SFX)
    {
        LoadObject($"Audio/SFX/{SFX}");
    }

    public void PlaySong(string songName)
    {
        LoadObject($"Audio/Music/{songName}");
    }

    public void StopSong()
    {
    }

    private void LoadActor(string actorName)
    {
        LoadObject($"Actors/{actorName}");
    }
    
    private void LoadEvidence(string evidenceName)
    {
        LoadObject($"Evidence/{evidenceName}");
    }
    
    private void LoadScene(string sceneName)
    {
        
    }

    private void LoadObject(string path)
    {
        try
        {
            _objectStorage.Add(_objectLoader.Load(path));
        }
        catch (NullReferenceException exception)
        {
            Debug.LogWarning($"{exception.GetType().Name}: Object at path {path} could not be loaded");
        }
    }
}