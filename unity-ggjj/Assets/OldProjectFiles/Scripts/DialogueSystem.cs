using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    // SINGLETON DEFINITION -----------------------------------------------------------------------------------------------
    public static DialogueSystem instance;
    void Awake()
    {
        instance = this;
        //REMOVE BEFORE RELEASE -------------------------------------------------------------------------------------------
        isWaitingForUserInput = true;
    }

    // MY VARIABLES -------------------------------------------------------------------------------------------------------
    // "speechPanel" is the box where all the text is written on.
    public GameObject speechPanel;
    // "speakerNameText" is specifically the text that specifies the speaker of a sentence.
    public TextMeshProUGUI speakerNameText;
    // "speechText" is, well, the text that is been said.
    public TextMeshProUGUI speechText;

    public List<Sprite> speechBoxes;

    // FUNCTIONS ----------------------------------------------------------------------------------------------------------
    // Override removes the previous text from screen.
    public void SayOverride(string speech, string speaker, float speed)
    {
        speaking = StartCoroutine(Speaking(speech, "", speaker, false, speed));
    }

    // Additive keeps the previous text on the screen.
    public void SayAdditive(string speech, string previousSpeech, string speaker, float speed)
    {
        speaking = StartCoroutine(Speaking(speech, previousSpeech, speaker, true, speed));
    }

    // This function determines the speaker by checking whether or not the new speaker is different than the current
    // speaker. ...Or whether the new speaker is not specified.
    string DetermineSpeaker(string s)
    {
        if (s != speakerNameText.text)
        {
            return s;
        }
        else
        {
            return speakerNameText.text;
        }
    }

    // This function disables the speech panel.
    public void Close()
    {
        speechPanel.SetActive(false);
    }

    public bool isSpeaking { get { return speaking != null; } }
    [HideInInspector] public bool isWaitingForUserInput;

    Coroutine speaking = null;

    public TextArchitect currentArchitect = null;

    IEnumerator Speaking(string speech, string previousSpeech, string speaker, bool additive, float speed)
    {
        speechPanel.SetActive(true);

        /*
        string additiveSpeech = additive ? speechText.text : "";
        targetSpeech = additiveSpeech + speech;
        */

        if(currentArchitect == null)
        {
            currentArchitect = new TextArchitect(speechText, speech, previousSpeech, 1, speed);
        }
        else
        {
            currentArchitect.Renew(speech, previousSpeech);
        }

        speakerNameText.text = DetermineSpeaker(speaker);
        isWaitingForUserInput = false;

        while (currentArchitect.isConstructing)
        {
            /*
            if (Input.GetKey(KeyCode.Space))
            {
                currentArchitect.skip = true;
            }
            */
            yield return new WaitForEndOfFrame();
        }

        isWaitingForUserInput = true;
        /*
        while (isWaitingForUserInput)
            yield return new WaitForEndOfFrame();
        */
        StopSpeaking();
    }

    public void StopSpeaking()
    {
        if (isSpeaking)
        {
            StopCoroutine(speaking);
        }
        if (currentArchitect != null && currentArchitect.isConstructing)
        {
            currentArchitect.Stop();
        }

        if (speakerNameText.text != "" && speakerNameText.text != "???" && speakerNameText.text != null)
        {
            CharacterManager.instance.GetCharacter(speakerNameText.text).ChangeToIdle();
        }
        speaking = null;
    }

    public void SetSpeechPanel(string characterName)
    {
        if (characterName == "Anon")
        {
            speechPanel.GetComponent<Image>().sprite = speechBoxes[0];
        }
        else if (characterName == "???")
        {
            speechPanel.GetComponent<Image>().sprite = speechBoxes[1];
        }
        else
        {
            speechPanel.GetComponent<Image>().sprite = speechBoxes.Find(x => x.name == ("TextBox" + characterName.Replace(" ", "")));
        }
    }
}