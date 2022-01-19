using System;
using System.Diagnostics;
using UnityEngine;

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

        public void Render()
        {
            throw new System.NotImplementedException();
        }
    }
}