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
            .LoadAll<TextAsset>("InkDialogueScripts/Case1").OrderBy(textAsset => textAsset.name.Split('-')[1].PadLeft(2, '0'))
                .Select(narrativeScript => new TestCaseData(narrativeScript).SetName(narrativeScript.name).Returns(null));

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
            storyProgresser.Setup();

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
                    yield return ProgressStory();
                }
                else if (_narrativeScript.Story.currentChoices.Count > 0)
                {
                    yield return TestTools.WaitForState(() => !_appearingDialogueController.IsPrintingText);
                    
                    var choices = _narrativeScript.Story.currentChoices;
                    var currentText = _narrativeScript.Story.currentText;
                    var evidenceMenu = Object.FindObjectOfType<EvidenceMenu>();

                    if (choices.Count > 0 && !visitedChoices.ContainsKey(currentText) && (evidenceMenu == null || !evidenceMenu.CanPresentEvidence))
                    {
                        visitedChoices.Add(currentText, new Choice[choices.Count]);
                    }
                    else if (evidenceMenu != null && evidenceMenu.CanPresentEvidence)
                    {
                        foreach (var choice in choices)
                        {
                            yield return storyProgresser.SelectChoice(choice.index, _narrativeScriptPlayer.GameMode, new EvidenceAssetName(choice.text));
                        }
                        continue;
                    }
                    
                    var possibleChoices = choices.Where(choice => visitedChoices[currentText].All(item => item == null || choice.text != item.text)).ToArray();
                    if (possibleChoices.Length > 0)
                    {
                        foreach (var choice in possibleChoices)
                        {
                            if (choice.index == 1 && _narrativeScript.Story.currentTags.Contains("correct") && visitedChoices.Values.Any(choiceList => choiceList.Any(choiceInList => choiceInList == null && choiceList != visitedChoices[currentText])))
                            {
                                yield return storyProgresser.SelectChoice(0, _narrativeScriptPlayer.GameMode, null);
                                continue;
                            }
                            
                            if (_narrativeScript.Story.currentTags.Count > 0 && int.TryParse(_narrativeScript.Story.currentTags[0], out var correctChoice) && choice.index == correctChoice && visitedChoices[currentText].Count(visitedChoice => visitedChoice == null) != 1)
                            {
                                continue;
                            }
                            visitedChoices[currentText][Array.FindIndex(visitedChoices[currentText], i => i == null)] = choice;
                            yield return storyProgresser.SelectChoice(choice.index, _narrativeScriptPlayer.GameMode, new EvidenceAssetName(choice.text));
                            break;
                        }
                    }
                    else
                    {
                        yield return storyProgresser.SelectChoice(0, _narrativeScriptPlayer.GameMode, null);
                    }
                }
                else
                {
                    break;
                }
                yield return null;
            }
            
            storyProgresser.TearDown();
        }

        private bool NarrativeScriptHasChanged(NarrativeScript narrativeScript)
        {
            return TestTools.GetField<NarrativeScript>(_narrativeScriptPlayer, "_activeNarrativeScript") != narrativeScript;
        }

        private IEnumerator ProgressStory()
        {
            var input = GameObject.Find("GameInput").GetComponent<InputModule>();
            if (!input.enabled) { yield break; }
            _appearingDialogueController.SpeedMultiplier = 8;
            yield return TestTools.WaitForState(() => !_narrativeGameState.AppearingDialogueController.IsPrintingText);
            _appearingDialogueController.SpeedMultiplier = 1;
            _narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.Continue();
        }
    }
}
