using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(SceneLoader))]
public class StoryController : MonoBehaviour
{
    [Tooltip("List of inky dialogue scripts to be played in order")]
    [SerializeField] private List<Dialogue> _dialogueList;
    
    [Tooltip("Write the name of the next scene to load here.")]
    [SerializeField] private string _nextSceneName;
    
    [Header("Events")]
    [SerializeField] private UnityEvent<Dialogue> _onNextDialogueScript;
    [SerializeField] private UnityEvent _onCrossExaminationStart;

    private SceneLoader _sceneLoader;
    private int _currentStory = -1;

    /// <summary>
    /// Initializes variables
    /// </summary>
    private void Awake()
    {
        _sceneLoader = GetComponent<SceneLoader>();
    }

    /// <summary>
    /// Starts the first dialogue script.
    /// This script should always be last in the run order in order for this to work properly.
    /// </summary>
    private void Start()
    {
        if (_dialogueList.Count == 0)
        {
            Debug.LogWarning("No narrative scripts assigned to StoryController", this);
        }
        else
        {
            RunNextDialogueScript();
        }
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
                _sceneLoader.LoadScene(_nextSceneName);
        }
        else
        {
            if (_dialogueList[_currentStory].ScriptMode == DialogueControllerMode.CrossExamination)
            {
                _onCrossExaminationStart.Invoke();
            }
            _onNextDialogueScript.Invoke(_dialogueList[_currentStory]);
        }
    }
}
