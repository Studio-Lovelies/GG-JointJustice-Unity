using UnityEngine;

namespace Credits.Renderables
{
    public interface IRenderable
    {
        public GameObject Render(GameObject prefab);
    }
}