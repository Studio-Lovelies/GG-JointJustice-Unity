using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

public class AppearingDialogueControllerTest
{
    private const string TEST_TEXT =
        "Lorem ipsum dolor sit amet consectetur adipiscing elit Integer luctus leo nec mi pulvinar eget molestie purus gravida Aliquam imperdiet pharetra massa vel aliquam Pellentesq";

    private AppearingDialogueController _appearingDialogueController;
    
    [UnitySetUp]
    public IEnumerator SetUp()
    {
        yield return SceneManager.LoadSceneAsync("Inky-TestScene", LoadSceneMode.Additive);
        _appearingDialogueController = Object.FindObjectOfType<AppearingDialogueController>();
    }

    [UnityTest]
    public IEnumerator DialogueAppearsAtCorrectSpeed()
    {
        const float INITIAL_CHARACTER_DELAY = 0.5f;
        const float DELAY_DECREMENT = 0.1f;
        const float WAIT_TIME = 5;

        for (float i = INITIAL_CHARACTER_DELAY; i > 0; i -= DELAY_DECREMENT)
        {
            _appearingDialogueController.CharacterDelay = i;
            _appearingDialogueController.PrintText(TEST_TEXT);

            yield return new WaitForSeconds(WAIT_TIME);
         
            Debug.Log(WAIT_TIME / i);
            Assert.AreEqual(Mathf.Ceil(WAIT_TIME / i), _appearingDialogueController.MaxVisibleCharacters);
        }
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        yield return SceneManager.UnloadSceneAsync("Inky-TestScene");
    }
}
