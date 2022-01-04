using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SceneLoader))]
public class StoryController : MonoBehaviour
{
    [Tooltip("List of inky dialogue scripts to be played in order")]
    [SerializeField] private List<NarrativeScript> _narrativeScripts;
    
    [Tooltip("Write the name of the next scene to load here.")]
    [SerializeField] private string _nextSceneName;
    
    [Header("Events")]
    [SerializeField] private UnityEvent<NarrativeScript> _onNextDialogueScript;
    [SerializeField] private UnityEvent _onCrossExaminationStart;

    private SceneLoader _sceneLoader;
    private int _currentStory = -1;

    /// <summary>
    /// Initializes variables
    /// </summary>
    private void Awake()
    {
        _sceneLoader = GetComponent<SceneLoader>();
        foreach (var narrativeScript in _narrativeScripts)
        {
            narrativeScript.Initialize();
        }
    }

    /// <summary>
    /// Starts the first dialogue script.
    /// This script should always be last in the run order in order for this to work properly.
    /// </summary>
    private void Start()
    {
        if (_narrativeScripts.Count == 0)
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
        if (_narrativeScripts.Count <= 0)
            return;

        _currentStory++;
        if (_currentStory >= _narrativeScripts.Count)
        {
            if (!_sceneLoader.Busy)
                _sceneLoader.LoadScene(_nextSceneName);
        }
        else
        {
            if (_narrativeScripts[_currentStory].Type == DialogueControllerMode.CrossExamination)
            {
                _onCrossExaminationStart.Invoke();
            }
            _onNextDialogueScript.Invoke(_narrativeScripts[_currentStory]);
        }
    }
}
