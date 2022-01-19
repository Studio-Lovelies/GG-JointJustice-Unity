using System.Linq;
using UnityEngine;

namespace Credits
{
    public class Parser : MonoBehaviour
    {
        public TextAsset _textAsset;
        public void Start()
        {
            var a= Credits.Generator.GenerateFromMarkdown(_textAsset.text).ToList();
            var b = 3;

        }
    }
}
