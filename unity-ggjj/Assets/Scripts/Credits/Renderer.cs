using System;
using System.Linq;
using Credits.Renderables;
using UnityEngine;

namespace Credits
{
    public class Renderer : MonoBehaviour
    {
        public TextAsset sourceTextAsset;

        public GameObject textPrefab;
        public GameObject imagePrefab;
        private const int Y_OFFSET_PER_LINE_AT720_P = 64;

        private static float YOffsetPerLineAtCurrentResolution => Y_OFFSET_PER_LINE_AT720_P / 720f * Screen.height;
        public void Start()
        {
            var linesAsGameObjects= Credits.Generator.GenerateFromMarkdown(sourceTextAsset.text).Select((renderable) => {
                if (renderable.GetType() == typeof(StyledString))
                {
                    var item = renderable.Render(textPrefab);
                    item.transform.SetParent(transform.parent);
                    return item;
                }

                if (renderable.GetType() == typeof(Image))
                {
                    var item = renderable.Render(imagePrefab);
                    item.transform.SetParent(transform.parent);
                    return item;
                }

                throw new NotSupportedException($"Rendering of data of type {renderable.GetType()} isn't supported yet");
            }).ToArray();
            for (var i = 0; i < linesAsGameObjects.Length; i++)
            {
                var rectTransform = linesAsGameObjects[i].GetComponent<RectTransform>();
                rectTransform.SetParent(transform);
                rectTransform.position -= new Vector3(0, YOffsetPerLineAtCurrentResolution * i, 0);
            }
        }
    }
}
