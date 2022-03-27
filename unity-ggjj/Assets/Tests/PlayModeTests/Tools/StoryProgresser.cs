using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Tests.PlayModeTests.Tools
{
    public class StoryProgresser : InputTestTools
    {
        /// <summary>
        /// Holds the X Key until an AppearingDialogueController is not printing text
        /// </summary>
        public IEnumerator ProgressStory()
        {
            Press(Keyboard.xKey);
            var appearingDialogueController = Object.FindObjectOfType<AppearingDialogueController>();
            yield return TestTools.WaitForState(() => !appearingDialogueController.IsPrintingText);
            Release(Keyboard.xKey);
        }

        /// <summary>
        /// Selects a choice and presses the x key to progress the story
        /// </summary>
        /// <param name="choiceIndex">The index of the choice to select</param>
        /// <param name="gameMode">The GameMode the game is currently using</param>
        /// <param name="evidenceName">The name of any evidence to present (if any)</param>
        public IEnumerator SelectChoice(int choiceIndex, GameMode gameMode, EvidenceAssetName evidenceName)
        {
            switch (gameMode)
            {
                case GameMode.Dialogue:
                    var choice = Object.FindObjectOfType<ChoiceMenu>().transform.GetChild(choiceIndex).GetComponent<Selectable>();
                    choice.Select();
                    yield return PressForFrame(Keyboard.xKey);
                    Debug.Log(choiceIndex);
                    break;
                case GameMode.CrossExamination:
                    yield return choiceIndex switch
                    {
                        0 => PressForFrame(Keyboard.xKey),
                        1 => PressForFrame(Keyboard.cKey),
                        2 =>  SelectEvidence(evidenceName),
                        _ => throw new ArgumentException($"Choice index can only be 0, 1, or 2 in GameMode {gameMode}")
                    };
                    break;
                default:
                    throw new NotSupportedException($"GameMode '{gameMode}' is not supported");
            }
        }

        private IEnumerator SelectEvidence(EvidenceAssetName evidenceName)
        {
            yield return PressForFrame(Keyboard.zKey);
            var evidenceMenu = Object.FindObjectOfType<EvidenceMenu>();
            var evidenceMenuItems = evidenceMenu.transform.GetComponentsInChildren<EvidenceMenuItem>();
            var incrementButton = evidenceMenu.transform.GetComponentsInChildren<Selectable>().First(menuItem => menuItem.name == "IncrementButton");

            for (var i = 0; i < TestTools.GetField<int>(evidenceMenu, "_numberOfPages"); i++)
            {
                foreach (var evidenceMenuItem in evidenceMenuItems)
                {
                    if (evidenceMenuItem.CourtRecordObject.InstanceName != evidenceName)
                    {
                        continue;
                    }

                    evidenceMenuItem.GetComponent<Selectable>().Select();
                    yield return PressForFrame(Keyboard.xKey);
                    yield break;
                }
                
                incrementButton.Select();
            }

            throw new MissingReferenceException($"Evidence menu did not contain {evidenceName}");
        }
    }
}