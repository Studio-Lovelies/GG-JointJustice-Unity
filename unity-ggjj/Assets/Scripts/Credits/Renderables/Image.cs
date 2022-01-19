using System;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Credits.Renderables
{
    [DebuggerDisplay("[img]: {_path}")]
    public class Image : IRenderable
    {
        private readonly string _path;
        public Sprite image;

        public Image(string path)
        {
            _path = path;
            image = Resources.Load<Sprite>(path);
            if (image == null)
            {
                throw new NullReferenceException($"Cannot find image at absolute path '{path}'");
            }
        }

        public GameObject Render(GameObject prefab)
        {
            var instance = Object.Instantiate(prefab);
            var imageComponent = instance.GetComponent<UnityEngine.UI.Image>();
            imageComponent.sprite = image;
            var rectTransform = instance.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(image.texture.width / image.pixelsPerUnit, image.texture.height / image.pixelsPerUnit);
            return instance;
        }
    }
}