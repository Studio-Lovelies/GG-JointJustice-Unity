using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Testing : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NovelController.instance.LoadChapterFile("turnabout1_trialPart1");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X) && DialogueSystem.instance.isWaitingForUserInput)
        {
            if (!CourtRecordManager.instance.courtRecordOpen.activeInHierarchy)
            {
                NovelController.instance.Next();
            }
            else
            {
                if (!CourtRecordManager.instance.isSelected && EventSystem.current.currentSelectedGameObject != null)
                {
                    CourtRecordManager.instance.SelectEvidence(EventSystem.current.currentSelectedGameObject.GetComponent<Evidence>());
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Z) && DialogueSystem.instance.isWaitingForUserInput)
        {
            if (!CourtRecordManager.instance.isSelected)
            {
                if (CourtRecordManager.instance.courtRecordOpen.activeInHierarchy)
                {
                    CourtRecordManager.instance.BackButton();
                }
                else
                {
                    CourtRecordManager.instance.CourtRecordButton();
                }
            }
            else
            {
                CourtRecordManager.instance.BackButton();
            }
        }
    }
}
