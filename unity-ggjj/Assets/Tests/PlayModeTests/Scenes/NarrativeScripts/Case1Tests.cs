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
        private NarrativeGameState _narrativeGameState;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return SceneManager.LoadSceneAsync("Game");
        }

        [UnityTest]
        [Timeout(1000000)]
        [TestCaseSource(nameof(NarrativeScripts))]
        public IEnumerator NarrativeScriptsRunWithNoErrors(TextAsset narrativeScriptText)
        {
            _narrativeGameState = Object.FindObjectOfType<NarrativeGameState>();
            _narrativeScript = new NarrativeScript(narrativeScriptText);
            _narrativeGameState.NarrativeScriptStorage.NarrativeScript = _narrativeScript;
            _narrativeGameState.StartGame();
            _narrativeScriptPlayer = _narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer;

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
                    if (visitedChoices.Count != 0 && visitedChoices.Values.All(choiceArray => choiceArray.Any(choice => choice == null)))
                    {
                        _narrativeScriptPlayer.ActiveNarrativeScript = _narrativeScript;
                        currentChoiceList = 0;
                        _narrativeScript.Reset();
                        _narrativeGameState.BGSceneList.InstantiateBGScenes(_narrativeScript);
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
                    var choices = _narrativeScript.Story.currentChoices;
                    if (choices.Count > 0)
                    {
                        currentChoiceList++;
                        if (!visitedChoices.ContainsKey(currentChoiceList))
                        {
                            visitedChoices.Add(currentChoiceList, new Choice[choices.Count]);
                        }
                    }

                    foreach (var choice in choices.Where(choice => visitedChoices[currentChoiceList].All(item => item == null || choice.text != item.text)))
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
