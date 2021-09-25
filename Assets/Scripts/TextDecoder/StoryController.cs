using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SceneLoader))]
public class StoryController : MonoBehaviour
{
    [SerializeField] private List<Dialogue> _dialogueList;

    [Header("Events")]

    [SerializeField] private UnityEvent<Dialogue> _onNextDialogueScript;


    private SceneLoader _sceneLoader;

    private int _currentStory = -1;

    /// <summary>
    /// Initializes the script and starts the first dialogue script.
    /// This script should always be last in the run order in order for this to work properly.
    /// </summary>
    void Start()
    {
        if (_dialogueList.Count == 0)
        {
            Debug.LogError("This unity scene does not have any dialogue scripts");
            return;
        }
        _sceneLoader = GetComponent<SceneLoader>();
        RunNextDialogueScript();
    }

    /// <summary>
    /// Loads the next dialogue script. If it doesn't exist, loads the next unity scene.
    /// </summary>
    public void RunNextDialogueScript()
    {
        if (_dialogueList.Count <= 0)
            return;

        _currentStory++;
        if (_currentStory >= _dialogueList.Count)
        {
            if (!_sceneLoader.Busy)
                _sceneLoader.ChangeSceneBySceneName();
        }
        else
        {
            _onNextDialogueScript.Invoke(_dialogueList[_currentStory]);
        }
    }

}

[System.Serializable]
public struct Dialogue
{
    public Dialogue(TextAsset script, DialogueControllerMode type)
    {
        DialogueScript = script;
        ScriptType = type;
    }

    public TextAsset DialogueScript;
    public DialogueControllerMode ScriptType;
}
