using System;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Credits.Renderables
{
    [DebuggerDisplay("[txt] h{style.heading}: '{text}'")]
    public class StyledString : IRenderable
    {
        private readonly static int[] sizeAt720pForHeading = new int[]{48,64};

        public class Style
        {
            public int heading;
        }

        public StyledString(string rawString)
        {
            style = new Style();
            text = "";
            if (string.IsNullOrEmpty(rawString))
            {
                return;
            }

            var currentIndex = 0;
            while (rawString[currentIndex] == '#')
            {
                ++currentIndex;
            }
            style.heading = currentIndex;
            text = rawString.Substring(style.heading).Trim();
        }

        public string text;
        public Style style;
        public GameObject Render(GameObject prefab)
        {
            if (sizeAt720pForHeading.Length <= style.heading)
            {
                throw new IndexOutOfRangeException($"Size for text of heading '{style.heading}' isn't defined");
            }

            var instance = Object.Instantiate(prefab);
            var rectTransform = instance.GetComponent<RectTransform>();
            var textMeshComponent = instance.GetComponent<TextMeshProUGUI>();
            textMeshComponent.fontSize = sizeAt720pForHeading[style.heading] / 720f * Screen.height;
            textMeshComponent.text = text;
            rectTransform.sizeDelta = new Vector3(Screen.width, textMeshComponent.fontSize, 0);
            return instance;
        }
    }
}