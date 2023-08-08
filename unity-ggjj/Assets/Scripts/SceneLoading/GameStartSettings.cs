using UnityEngine;

namespace SceneLoading
{
    [CreateAssetMenu(menuName = "Game Start Settings", fileName = "New game start settings")]
    public class GameStartSettings : ScriptableObject
    {
        [field: SerializeField] public TextAsset NarrativeScriptTextAsset { get; set; }
    }
}
