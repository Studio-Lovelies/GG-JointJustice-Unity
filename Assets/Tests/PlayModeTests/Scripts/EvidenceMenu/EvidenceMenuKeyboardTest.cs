using System.Collections;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayModeTests.Scripts.EvidenceMenu
{
    public class EvidenceMenuKeyboardTest
    {
        private readonly InputTestTools _inputTestTools = new InputTestTools();
        private global::EvidenceMenu _evidenceMenu;

        [UnityTest, Order(0)]
        public IEnumerator EvidenceMenuOpensAndCloses()
        {
            SceneManager.LoadScene("EvidenceMenu - Test Scene", LoadSceneMode.Single);
            yield return null;
            _evidenceMenu = Resources.FindObjectsOfTypeAll<global::EvidenceMenu>()[0];
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.zKey);
            Assert.True(_evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.cKey);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.zKey);
            Assert.False(_evidenceMenu.isActiveAndEnabled);
        }

        [UnityTest, Order(1)]
        public IEnumerator EvidenceMenuCannotBeClosedWhenPresentingEvidence()
        {
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(_evidenceMenu, _inputTestTools.Keyboard.xKey);
            Assert.True(_evidenceMenu.isActiveAndEnabled);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.zKey);
            Assert.True(_evidenceMenu.isActiveAndEnabled);
        }

        [UnityTest, Order(2)]
        public IEnumerator EvidenceCanBeSelected()
        {
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.enterKey);
            Assert.False(_evidenceMenu.isActiveAndEnabled);
        }
        
        [UnityTest, Order(3)]
        public IEnumerator CanNavigateWithLeftAndRightArrows()
        {
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(_evidenceMenu, _inputTestTools.Keyboard.xKey);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.leftArrowKey);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.leftArrowKey);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.rightArrowKey);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.rightArrowKey);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.rightArrowKey);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.leftArrowKey);

            Assert.AreEqual("Bent Coins", _evidenceMenu.GetComponent<Menu>().SelectedButton.GetComponent<EvidenceMenuItem>().Evidence.DisplayName);
        }
        
        [UnityTest, Order(4)]
        public IEnumerator CanNavigateToMultiplePages()
        {
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.enterKey);
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(_evidenceMenu, _inputTestTools.Keyboard.xKey);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.enterKey);
            yield return _inputTestTools.WaitForBehaviourActiveAndEnabled(_evidenceMenu, _inputTestTools.Keyboard.xKey);

            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.leftArrowKey);
            yield return _inputTestTools.RepeatPressForFrames(_inputTestTools.Keyboard.enterKey, 50);
            yield return _inputTestTools.RepeatPressForFrames(_inputTestTools.Keyboard.rightArrowKey, 10);
            yield return _inputTestTools.RepeatPressForFrames(_inputTestTools.Keyboard.enterKey, 101);
            yield return _inputTestTools.RepeatPressForFrames(_inputTestTools.Keyboard.leftArrowKey, 4);

            yield return null;
            
            Assert.AreEqual("Jory Sr's Letter",_evidenceMenu.GetComponent<Menu>().SelectedButton.GetComponent<EvidenceMenuItem>().Evidence.DisplayName);
        }

        [UnityTest, Order(5)]
        public IEnumerator ProfileMenuCanBeAccessed()
        {
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.cKey);
            Assert.AreEqual("Arin", _evidenceMenu.GetComponent<Menu>().SelectedButton.GetComponent<EvidenceMenuItem>().Evidence.DisplayName);
            yield return _inputTestTools.RepeatPressForFrames(_inputTestTools.Keyboard.rightArrowKey, 5);
            Assert.AreEqual("Ross", _evidenceMenu.GetComponent<Menu>().SelectedButton.GetComponent<EvidenceMenuItem>().Evidence.DisplayName);
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.cKey);
            Assert.AreEqual("Bent Coins", _evidenceMenu.GetComponent<Menu>().SelectedButton.GetComponent<EvidenceMenuItem>().Evidence.DisplayName);
        }

        [UnityTest, Order(5)]
        public IEnumerator EvidenceCanBeSubstitutedWithAltEvidence()
        {
            yield return _inputTestTools.PressForFrame(_inputTestTools.Keyboard.enterKey);
            
            while (!_evidenceMenu.isActiveAndEnabled)
            {
                yield return _inputTestTools.PressForSeconds(_inputTestTools.Keyboard.xKey, 0.5f);
            }
            
            Assert.AreEqual("Jory's Backpack", _evidenceMenu.GetComponent<Menu>().SelectedButton.GetComponent<EvidenceMenuItem>().Evidence.DisplayName);
        }
    }
}
