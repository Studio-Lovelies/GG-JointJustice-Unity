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
        private AppearingDialogueController _appearingDialogueController;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return SceneManager.LoadSceneAsync("Game");
            _appearingDialogueController = Object.FindObjectOfType<AppearingDialogueController>();
            _narrativeGameState = Object.FindObjectOfType<NarrativeGameState>();

        }

        [UnityTest]
        [Timeout(1000000)]
        [TestCaseSource(nameof(NarrativeScripts))]
        public IEnumerator NarrativeScriptsRunWithNoErrors(TextAsset narrativeScriptText)
        {
            _narrativeScript = new NarrativeScript(narrativeScriptText);
            _narrativeGameState.NarrativeScriptStorage.NarrativeScript = _narrativeScript;
            _narrativeGameState.StartGame();
            _narrativeScriptPlayer = _narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer;

            yield return PlayNarrativeScript();
        }

        private IEnumerator PlayNarrativeScript()
        {
            var visitedChoices = new Dictionary<string, Choice[]>();
            var storyProgresser = new StoryProgresser();

            while (true)
            {
                if (NarrativeScriptHasChanged(_narrativeScript))
                {
                    if (visitedChoices.Count != 0 && visitedChoices.Values.SelectMany(choices => choices).Any(choice => choice == null))
                    {
                        _narrativeScriptPlayer.ActiveNarrativeScript = _narrativeScript;
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
                    yield return TestTools.WaitForState(() => !_appearingDialogueController.IsPrintingText);
                    
                    var choices = _narrativeScript.Story.currentChoices;
                    var currentText = _narrativeScript.Story.currentText;
                    if (choices.Count > 0 && !visitedChoices.ContainsKey(currentText))
                    {
                        visitedChoices.Add(currentText, new Choice[choices.Count]);
                    }

                    var possibleChoices = choices.Where(choice => visitedChoices[currentText].All(item => item == null || choice.text != item.text)).ToArray();
                    if (possibleChoices.Length > 0)
                    {
                        foreach (var choice in possibleChoices)
                        {
                            if (choice.index == 1 && _narrativeScript.Story.currentTags.Contains("correct") && visitedChoices.Values.Any(choiceList => choiceList.Any(choice => choice == null && choiceList != visitedChoices[currentText])))
                            {
                                yield return storyProgresser.SelectChoice(0, _narrativeScriptPlayer.GameMode);
                                continue;
                            }
                            
                            visitedChoices[currentText][Array.FindIndex(visitedChoices[currentText], i => i == null)] =
                                choice;
                            yield return storyProgresser.SelectChoice(choice.index, _narrativeScriptPlayer.GameMode);
                            break;
                        }
                    }
                    else
                    {
                        yield return storyProgresser.SelectChoice(0, _narrativeScriptPlayer.GameMode);
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
