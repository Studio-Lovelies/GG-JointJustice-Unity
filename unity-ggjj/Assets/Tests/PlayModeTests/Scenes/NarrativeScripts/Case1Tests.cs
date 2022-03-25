using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using NUnit.Framework;
using Tests.PlayModeTests.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Scenes.NarrativeScripts
{
    public class Case1Tests
    {
        private static IEnumerable<TestCaseData> NarrativeScripts => Resources
            .LoadAll<TextAsset>("InkDialogueScripts/Case1").Select(narrativeScript =>
                new TestCaseData(narrativeScript).SetName(narrativeScript.name).Returns(null));

        private NarrativeScript _narrativeScript;
        private INarrativeScriptPlayer _narrativeScriptPlayer;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return SceneManager.LoadSceneAsync("Game");
        }

        [UnityTest]
        [TestCaseSource(nameof(NarrativeScripts))]
        public IEnumerator NarrativeScriptsRunWithNoErrors(TextAsset narrativeScriptText)
        {
            var narrativeGameState = Object.FindObjectOfType<NarrativeGameState>();
            _narrativeScript = new NarrativeScript(narrativeScriptText);
            narrativeGameState.NarrativeScriptStorage.NarrativeScript = _narrativeScript;
            narrativeGameState.StartGame();
            _narrativeScriptPlayer = narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer;

            yield return PlayNarrativeScript();
        }

        private IEnumerator PlayNarrativeScript()
        {
            var visitedChoices = new Dictionary<int, Choice[]>();
            var storyProgresser = new StoryProgresser();
            var currentChoiceList = 0;

            while (true)
            {
                if (NarrativeScriptHasChanged(_narrativeScript))
                {
                    if (visitedChoices.Values.All(choiceArray => choiceArray.Any(choice => choice == null)))
                    {
                        _narrativeScriptPlayer.ActiveNarrativeScript = _narrativeScript;
                        currentChoiceList = 0;
                        _narrativeScript.Reset();
                    }
                    else
                    {
                        break;
                    }
                }
                
                if (_narrativeScript.Story.canContinue)
                {
                    yield return storyProgresser.ProgressStory();
                }
                else
                {
                    var currentChoices = _narrativeScript.Story.currentChoices;
                    if (currentChoices.Count > 0)
                    {
                        currentChoiceList++;
                        if (!visitedChoices.ContainsKey(currentChoiceList))
                        {
                            visitedChoices.Add(currentChoiceList, new Choice[currentChoices.Count]);
                        }
                    }

                    foreach (var choice in currentChoices.Where(choice => visitedChoices[currentChoiceList].All(item => item == null || choice.text != item.text)))
                    {
                        visitedChoices[currentChoiceList][Array.FindIndex(visitedChoices[currentChoiceList], i => i == null)] = choice;
                        yield return storyProgresser.SelectChoice(choice.index);
                        break;
                    }
                }
                yield return null;
            }
        }

        private bool NarrativeScriptHasChanged(NarrativeScript narrativeScript)
        {
            return TestTools.GetField<NarrativeScript>(_narrativeScriptPlayer, "_activeNarrativeScript") != narrativeScript;
        }
    }
}
