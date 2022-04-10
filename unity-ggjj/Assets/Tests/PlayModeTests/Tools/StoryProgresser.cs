using System;
using System.Collections;
using System.Linq;
using UnityEngine;
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
            Press(keyboard.xKey);
            var appearingDialogueController = Object.FindObjectOfType<AppearingDialogueController>();
            yield return TestTools.WaitForState(() => !appearingDialogueController.IsPrintingText);
            Release(keyboard.xKey);
        }

        /// <summary>
        /// Find the choice in the choice menu and press it
        /// </summary>
        /// <param name="choiceIndex">The index of the choice to press</param>
        public IEnumerator SelectDialogueChoice(int choiceIndex)
        {
            var choice = Object.FindObjectOfType<ChoiceMenu>().transform.GetChild(choiceIndex).GetComponent<Selectable>();
            choice.Select();
            yield return PressForFrame(keyboard.xKey);
        }

        /// <summary>
        /// Determine whether the player is being forced to present evidence
        /// or is cross examining a witness and select a choice accordingly
        /// </summary>
        /// <param name="crossExaminationChoice">The choice being made in the cross examination</param>
        /// <param name="evidenceName">The name of any evidence being presented</param>
        public IEnumerator SelectCrossExaminationChoice(CrossExaminationChoice crossExaminationChoice, EvidenceAssetName evidenceName)
        {
            // Otherwise we're in a cross examination. We can choose to continue, press the witness, or present evidence
            yield return crossExaminationChoice switch
            {
                CrossExaminationChoice.ContinueStory => PressForFrame(keyboard.xKey),
                CrossExaminationChoice.PressWitness => PressForFrame(keyboard.cKey),
                CrossExaminationChoice.PresentEvidence =>  PresentEvidence(evidenceName),
                _ => throw new ArgumentException("Choice index can only be 0, 1, or 2 in GameMode CrossExamination")
            };
        }

        /// <summary>
        /// Opens the evidence menu and finds a specified piece of evidence to present
        /// </summary>
        /// <param name="evidenceName">The name of the evidence to present</param>
        public IEnumerator PresentEvidence(EvidenceAssetName evidenceName)
        {
            var narrativeGameState = Object.FindObjectOfType<NarrativeGameState>();
            var courtRecordObjects = narrativeGameState.ObjectStorage.GetObjectsOfType<ICourtRecordObject>().ToList();

            // Check if the choice is the name of a piece of evidence (excludes incorrect options such as "wrong")
            if (courtRecordObjects.All(courtRecordObject => courtRecordObject.InstanceName != evidenceName))
            {
                yield break;
            }
            
            // Check if the item being presented in an actor
            if (courtRecordObjects.Any(courtRecordObject => courtRecordObject is ActorData && courtRecordObject.InstanceName == evidenceName))
            {
                yield return PressForFrame(keyboard.cKey);
            }
            
            yield return PressForFrame(keyboard.zKey);
            var evidenceMenu = Object.FindObjectOfType<EvidenceMenu>();
            var evidenceMenuItems = evidenceMenu.transform.GetComponentsInChildren<EvidenceMenuItem>();
            var incrementButton = evidenceMenu.transform.GetComponentsInChildren<Selectable>().First(menuItem => menuItem.name == "IncrementButton");

            // Loop through every page of the court record
            for (var i = 0; i < TestTools.GetField<int>(evidenceMenu, "_numberOfPages"); i++)
            {
                // Loop through every item and if it's the one we're looking for then press it
                foreach (var evidenceMenuItem in evidenceMenuItems)
                {
                    if (evidenceMenuItem.CourtRecordObject.InstanceName != evidenceName)
                    {
                        continue;
                    }
                    
                    evidenceMenuItem.GetComponent<Selectable>().Select();
                    yield return PressForFrame(keyboard.xKey);
                    yield break;
                }
                
                incrementButton.Select();
                yield return PressForFrame(keyboard.xKey);
            }

            throw new MissingReferenceException($"Evidence menu did not contain {evidenceName}");
        }
    }
}