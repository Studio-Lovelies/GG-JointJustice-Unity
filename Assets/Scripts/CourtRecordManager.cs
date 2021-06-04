using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CourtRecordManager : MonoBehaviour
{
    public static CourtRecordManager instance;
    void Awake()
    {
        instance = this;
        Close();
    }

    private GameObject obj = null;
    public TMP_Text myText;
    public List<GameObject> evidenceSlots;
    public GameObject courtRecordOpen;
    public GameObject courtRecordClosed;

    public GameObject evidencePanel;
    public GameObject evidencePanelName;
    public GameObject evidencePanelText;
    public GameObject evidencePanelEvidence;

    public GameObject toProfilesButton;
    public GameObject toEvidenceButton;

    public bool isSelected = false;

    private GameObject previouslySelectedItem;
    private bool firstEnter = false;

    // Update is called once per frame
    void Update()
    {
        if (courtRecordOpen.activeInHierarchy)
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                if (obj != EventSystem.current.currentSelectedGameObject)
                {
                    obj = EventSystem.current.currentSelectedGameObject;
                    myText.text = obj.GetComponent<Evidence>().evidenceName;
                }
            }
            else
            {
                myText.text = "";
            }
        }
    }

    public void Open()
    {
        courtRecordOpen.SetActive(true);
        courtRecordClosed.SetActive(false);
        ShowEvidences();
        HighlightFirstEvidence();
    }

    public void Close()
    {
        myText.text = "";
        for (int i = 0; i < 8; i++)
        {
            evidenceSlots[i].SetActive(false);
        }
        toProfilesButton.SetActive(true);
        toEvidenceButton.SetActive(false);
        courtRecordOpen.SetActive(false);
        courtRecordClosed.SetActive(true);
    }

    public void ShowEvidences()
    {
        Evidence aux;
        for (int i = 0; i < 8; i++)
        {
            evidenceSlots[i].SetActive(false);
            if (i < EvidenceManager.instance.playersCourtRecordEvidence.Count)
            {
                aux = EvidenceManager.instance.playersCourtRecordEvidence[i];
                evidenceSlots[i].SetActive(true);
                evidenceSlots[i].GetComponent<Evidence>().Initialize(aux.evidenceName, aux.evidenceDescription, aux.evidenceSprite, aux.isProfile);
            }
        }
    }

    public void ShowProfiles()
    {
        Evidence aux;
        for (int i = 0; i < 8; i++)
        {
            evidenceSlots[i].SetActive(false);
            if (i < EvidenceManager.instance.playersCourtRecordProfiles.Count)
            {
                aux = EvidenceManager.instance.playersCourtRecordProfiles[i];
                evidenceSlots[i].SetActive(true);
                evidenceSlots[i].GetComponent<Evidence>().Initialize(aux.evidenceName, aux.evidenceDescription, aux.evidenceSprite, aux.isProfile);
            }
        }
    }

    private void HighlightFirstEvidence()
    {
        firstEnter = true;
        if (evidenceSlots[0].activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(evidenceSlots[0]);
            myText.text = evidenceSlots[0].GetComponent<Evidence>().evidenceName;
        }
        firstEnter = false;
    }

    public void SelectEvidence(Evidence evidence)
    {
        AudioManager.instance.PlaySFX("selectBlip2", 1f);
        isSelected = true;
        evidencePanel.SetActive(true);
        previouslySelectedItem = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(null);
        evidencePanelName.GetComponent<TextMeshProUGUI>().text = evidence.evidenceName;
        evidencePanelText.GetComponent<TextMeshProUGUI>().text = evidence.evidenceDescription;
        evidencePanelEvidence.GetComponent<Image>().sprite = evidence.evidenceSprite;
    }

    public void DeselectEvidence()
    {
        firstEnter = true;
        isSelected = false;
        evidencePanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(previouslySelectedItem);
        myText.text = previouslySelectedItem.GetComponent<Evidence>().evidenceName;
        firstEnter = false;
    }

    public void MoveThroughEvidence()
    {

        if (!firstEnter)
        {
            AudioManager.instance.PlaySFX("selectBlip", 1f);
        }
    }

    public void BackButton()
    {
        AudioManager.instance.PlaySFX("cancel", 1f);
        if (isSelected)
        {
            DeselectEvidence();
        }
        else
        {
            Close();
        }
    }

    public void ToProfilesButton()
    {
        AudioManager.instance.PlaySFX("scroll", 1f);
        ShowProfiles();
        HighlightFirstEvidence();
        toEvidenceButton.SetActive(true);
        toProfilesButton.SetActive(false);
    }

    public void ToEvidenceButton()
    {
        AudioManager.instance.PlaySFX("scroll", 1f);
        ShowEvidences();
        HighlightFirstEvidence();
        toProfilesButton.SetActive(true);
        toEvidenceButton.SetActive(false);
    }

    public void CourtRecordButton()
    {
        if (DialogueSystem.instance.isWaitingForUserInput)
        {
            AudioManager.instance.PlaySFX("scroll", 1f);
            Open();
        }
    }
}
