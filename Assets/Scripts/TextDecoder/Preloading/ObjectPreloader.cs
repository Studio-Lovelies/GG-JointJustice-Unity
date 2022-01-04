using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectPreloader : IActorController, ISceneController, IEvidenceController, IAudioController, IAppearingDialogueController
{
    public ObjectStorage<ActorData> ActorStorage { get; } = new ObjectStorage<ActorData>();
    public ObjectStorage<Evidence> EvidenceStorage { get; } = new ObjectStorage<Evidence>();
    public ObjectStorage<AudioClip> AudioStorage { get; } = new ObjectStorage<AudioClip>();

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

    public void AddEvidence(string evidence)
    {
        LoadEvidence(evidence);
    }

    public void RemoveEvidence(string evidence)
    {
    }

    public void AddToCourtRecord(string actorName)
    {
        LoadActor(actorName);
    }

    public void RequirePresentEvidence()
    {
    }

    public void SubstituteEvidenceWithAlt(string evidence)
    {
    }

    public void OnPresentEvidence(ICourtRecordObject evidence)
    {
    }
    
    public void PlaySfx(string SFX)
    {
        LoadObject(AudioStorage, $"Audio/SFX/{SFX}");
    }

    public void PlaySong(string songName)
    {
        LoadObject(AudioStorage, $"Audio/Music/{songName}");
    }

    public void StopSong()
    {
    }

    private void LoadActor(string actorName)
    {
        LoadObject(ActorStorage, $"Actors/{actorName}");
    }
    
    private void LoadEvidence(string evidenceName)
    {
        LoadObject(EvidenceStorage, $"Evidence/{evidenceName}");
    }
    
    private void LoadScene(string sceneName)
    {
        
    }

    private static void LoadObject<T>(ObjectStorage<T> objectStorage, string path) where T : Object
    {
        try
        {
            objectStorage.Add(Resources.Load<T>(path));
        }
        catch (NullReferenceException exception)
        {
            Debug.LogWarning($"{exception.GetType().Name}: {typeof(T)} at path {path} could not be loaded");
        }
    }

    public float CharacterDelay { get; set; }
    public float DefaultPunctuationDelay { get; set; }
    public bool SkippingDisabled { get; set; }
    public bool ContinueDialogue { get; set; }
    public bool AutoSkip { get; set; }
    public bool AppearInstantly { get; set; }
    public bool TextBoxHidden { get; set; }
}