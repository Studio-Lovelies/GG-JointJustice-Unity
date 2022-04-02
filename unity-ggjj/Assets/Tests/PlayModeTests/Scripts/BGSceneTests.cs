using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Tests.PlayModeTests.Scripts
{
    public class BGSceneTests
    {
        private BGScene _bgScene;

        [Test]
        public void ThrowsIfNoSupportForActorSlots()
        {
            var rootGameObject = new GameObject();
            var slot1GameObject = new GameObject("slot1").AddComponent<Actor>();
            slot1GameObject.transform.parent = rootGameObject.transform;

            _bgScene = rootGameObject.AddComponent<BGScene>();

            Assert.Throws<NotSupportedException>(() => { _bgScene.SetActiveActorSlot("NotFound"); });
            Assert.Throws<NotSupportedException>(() => { _bgScene.GetActorAtSlot("NotFound");     });
        }

        [Test]
        public void ThrowsIfNoSlotWithMatchingName()
        {
            var rootGameObject = new GameObject();
            var slot1GameObject = new GameObject("slot1").AddComponent<Actor>();
            var slot2GameObject = new GameObject("slot2").AddComponent<Actor>();
            slot1GameObject.transform.parent = rootGameObject.transform;
            slot2GameObject.transform.parent = rootGameObject.transform;

            _bgScene = rootGameObject.AddComponent<BGScene>();

            Assert.Throws<KeyNotFoundException>(() => { _bgScene.SetActiveActorSlot("NotFound"); });
            Assert.Throws<KeyNotFoundException>(() => { _bgScene.GetActorAtSlot("NotFound");     });
        }
    }
}
