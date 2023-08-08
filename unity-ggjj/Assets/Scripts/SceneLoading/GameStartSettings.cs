using Ink.Parsed;
using UnityEngine;

namespace SceneLoading
{
    [CreateAssetMenu(menuName = "Game Start Settings", fileName = "New game start settings")]
    public class GameStartSettings : ScriptableObject
    {
        private TextAsset _narrativeScriptTextAsset;
        public TextAsset NarrativeScriptTextAsset
        {
            get
            {
                var narrativeScriptTextAsset = _narrativeScriptTextAsset;
                _narrativeScriptTextAsset = null;
                return narrativeScriptTextAsset;
            }
            set => _narrativeScriptTextAsset = value;
        }
    }
}
