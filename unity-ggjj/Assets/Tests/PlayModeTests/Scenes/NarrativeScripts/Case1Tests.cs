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
        private StoryProgresser _storyProgresser;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return SceneManager.LoadSceneAsync("Game");
            _appearingDialogueController = Object.FindObjectOfType<AppearingDialogueController>();
            _narrativeGameState = Object.FindObjectOfType<NarrativeGameState>();
            _storyProgresser = new StoryProgresser();
            _storyProgresser.Setup();
        }

        [UnityTest]
        [Timeout(5 * 60 * 1000)]
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

            while (true)
            {
                _appearingDialogueController.AppearInstantly = true;
                _appearingDialogueController.SpeedMultiplier = 20;

                // If the narrative script has changed then we have reached the end of a script
                if (NarrativeScriptHasChanged(_narrativeScript))
                {
                    // Are there choices remaining to explore in the previous script?
                    // Then reload the previous script and explore the remaining choices
                    if (visitedChoices.Count != 0 && visitedChoices.Values.SelectMany(choices => choices).Any(choice => choice == null))
                    {
                        _narrativeScriptPlayer.ActiveNarrativeScript = _narrativeScript;
                        _narrativeScript.Reset();
                        _narrativeGameState.BGSceneList.InstantiateBGScenes(_narrativeScript);
                        continue;
                    }
                    
                    break;
                }

                // If the story can continue, then just continue
                if (_narrativeScript.Story.canContinue)
                {
                    yield return ProgressStory();
                    continue;
                }
                
                // No choices and cannot continue means the end of a story
                if (_narrativeScript.Story.currentChoices.Count == 0)
                {
                    break;
                }
                
                yield return TestTools.WaitForState(() => !_appearingDialogueController.IsPrintingText);
                
                var choices = _narrativeScript.Story.currentChoices;
                var currentText = _narrativeScript.Story.currentText;
                var evidenceMenu = Object.FindObjectOfType<EvidenceMenu>();

                // If the evidence menu is open we need to present evidence
                if (evidenceMenu != null && evidenceMenu.CanPresentEvidence)
                {
                    foreach (var choice in choices)
                    {
                        yield return _storyProgresser.PresentEvidence(new EvidenceAssetName(choice.text));
                    }
                    continue;
                }
                
                // If we encounter choices, add a new entry to the dictionary to store visited choices
                if (choices.Count > 0 && !visitedChoices.ContainsKey(currentText) && (evidenceMenu == null || !evidenceMenu.CanPresentEvidence))
                {
                    visitedChoices.Add(currentText, new Choice[choices.Count]);
                }

                // Get all the choices we have not visited yet
                var possibleChoices = choices.Where(choice => visitedChoices[currentText].All(item => item == null || choice.text != item.text)).ToArray();
                // Debug.Log(possibleChoices.Length);
                foreach (var choice in possibleChoices)
                {
                    // During cross examinations we want to select the choice marked "correct" last
                    if (choice.index == 1 && _narrativeScript.Story.currentTags.Contains("correct") && visitedChoices.Values.Any(choiceList => choiceList.Any(choiceInList => choiceInList == null && choiceList != visitedChoices[currentText])))
                    {
                        yield return _storyProgresser.SelectCrossExaminationChoice(CrossExaminationChoice.ContinueStory, null);
                        continue;
                    }
                    
                    // If there is no "correct" choice, but 3 choices and we're in CrossExamination mode and all other choices are visited we need to present evidence
                    if (_narrativeScript.Story.currentChoices.Count == 3 && _narrativeScriptPlayer.GameMode == GameMode.CrossExamination && !visitedChoices.Values.Any(choiceList => choiceList.Any(choiceInList => choiceInList == null && choiceList != visitedChoices[currentText])))
                    {
                        visitedChoices[currentText][Array.FindIndex(visitedChoices[currentText], i => i == null)] = choice;
                        yield return _storyProgresser.PresentEvidence(new EvidenceAssetName(choice.text));
                        continue;
                    }
                    
                    // During dialogue we want to select the choice indicated by the current tag (e.g., #0) last
                    if (_narrativeScript.Story.currentTags.Count > 0 && int.TryParse(_narrativeScript.Story.currentTags[0], out var correctChoice) && choice.index == correctChoice && visitedChoices[currentText].Count(visitedChoice => visitedChoice == null) != 1)
                    {
                        continue;
                    }
                    
                    // Set the corresponding choice in the dictionary to visited and select the choice
                    visitedChoices[currentText][Array.FindIndex(visitedChoices[currentText], i => i == null)] = choice;
                    yield return SelectChoice(choice.index);
                    break;
                }

                // If all choices visited, continue as normal
                if (possibleChoices.Length == 0)
                {
                    yield return SelectChoice(0);
                }
            }
            
            _storyProgresser.TearDown();
        }

        /// <summary>
        /// Calls SelectDialogueChoice or SelectCrossExaminationChoice
        /// depending on the current game mode
        /// </summary>
        /// <param name="choiceIndex">The index of the choice in the story</param>
        private IEnumerator SelectChoice(int choiceIndex)
        {
            yield return _narrativeScriptPlayer.GameMode switch
            {
                GameMode.Dialogue => _storyProgresser.SelectDialogueChoice(choiceIndex),
                GameMode.CrossExamination => _storyProgresser.SelectCrossExaminationChoice((CrossExaminationChoice)choiceIndex, null),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private bool NarrativeScriptHasChanged(NarrativeScript narrativeScript)
        {
            return TestTools.GetField<NarrativeScript>(_narrativeScriptPlayer, "_activeNarrativeScript") != narrativeScript;
        }

        /// <summary>
        /// Progresses the story without using the input system to avoid input errors
        /// </summary>
        private IEnumerator ProgressStory()
        {
            var input = GameObject.Find("GameInput").GetComponent<InputModule>();
            if (!input.enabled) { yield break; }
            
            yield return TestTools.WaitForState(() => !_narrativeGameState.AppearingDialogueController.IsPrintingText);
            _appearingDialogueController.SpeedMultiplier = 1;
            _narrativeGameState.NarrativeScriptPlayerComponent.NarrativeScriptPlayer.Continue();
        }
    }
}
