using System;
using UnityEngine;

namespace SceneLoading
{
    /// <summary>
    /// Used to transfer a narrative script text asset between scenes
    /// NarrativeScriptTextAsset can only be accessed once. It sets itself to null as changes can persist in the UnityEditor, causing unexpected behaviour
    /// When calling NarrativeScriptTextAsset, store its value in a local variable in order to access it multiple times
    /// </summary>
    [CreateAssetMenu(menuName = "Game Start Settings", fileName = "New game start settings")]
    public class GameStartSettings : ScriptableObject
    {
        private TextAsset _narrativeScriptTextAsset;

        public bool IsNarrativeScriptTextAssetAssigned => _narrativeScriptTextAsset != null;

        public TextAsset GetAndClearNarrativeScriptTextAsset()
        {
            if (_narrativeScriptTextAsset == null)
            {
                throw new InvalidOperationException("Stored narrative script text asset can only be accessed once. If you wish to access the narrative script text asset multiple times a reference must be stored locally.");
            }
            
            var narrativeScriptTextAsset = _narrativeScriptTextAsset;
            _narrativeScriptTextAsset = null;
            return narrativeScriptTextAsset;
        }

        public void SetNarrativeScriptTextAsset(TextAsset narrativeScriptTextAsset)
        {
            _narrativeScriptTextAsset = narrativeScriptTextAsset;
        }
    }
}
