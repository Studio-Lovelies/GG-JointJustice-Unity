using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace UnityEngine.UI
{
    [AddComponentMenu("UI/TextSizeGroup")]
    public class TextSizeGroup : MonoBehaviour
    {
        [SerializeField, Tooltip("All the scripts that should share the same size.")]
        TextMeshProUGUI[] _texts = null;
        float _fontSize = float.PositiveInfinity;
        Vector2Int _resolution = Vector2Int.zero;
        bool _firstTime = true;

        ///<summary>
        ///Start is called automatically in the game when object is created, after Awake but before the first Update.
        ///</summary>
        IEnumerator Start()
        {
            yield return null;
            StartCoroutine(Calculate(true));
        }

        ///<summary>
        ///Checks all the texts in _texts, and determines which one has the tinies size while using autosize. After that uses the same size for all of the texts.
        ///</summary>
        ///<param name = "wait">Should the method wait a frame when doing calculations. Method doesn't always work instantly, and requires to wait for a frame for tmpro to calculate autosize.</param>
        public IEnumerator Calculate(bool wait = false)
        {
            _resolution = new Vector2Int(Screen.width, Screen.height);

            foreach (TextMeshProUGUI t in _texts)
            {
                t.enableAutoSizing = true;
                t.ForceMeshUpdate();
            }

            if (_firstTime || wait)
            {
                _firstTime = false;
                yield return null;
            }

            foreach (TextMeshProUGUI t in _texts)
            {
                t.ForceMeshUpdate();
                if (t.fontSize < _fontSize)
                {
                    _fontSize = t.fontSize;
                }
                t.enableAutoSizing = false;
                t.ForceMeshUpdate();
            }

            foreach (TextMeshProUGUI t in _texts)
            {
                t.fontSize = _fontSize;
                t.ForceMeshUpdate();
            }
        }

        ///<summary>
        ///LateUpdate is called every frame after every update has beben called
        ///</summary>
        void LateUpdate()
        {
            //If the game resolution changes mid game, Recalculate the sizes.
            if (_resolution != new Vector2Int(Screen.width, Screen.height))
            {
                StartCoroutine(Calculate());
            }
        }

        ///<summary>
        ///Starts the recalculation.
        ///</summary>
        public void PublicCalculate()
        {
            StartCoroutine(Calculate());
        }
    }
}