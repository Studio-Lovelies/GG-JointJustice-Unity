using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceScreen : MonoBehaviour
{
    public static ChoiceScreen instance;
    void Awake()
    {
        instance = this;
        Hide();
    }

    public GameObject choicePrefab;

    List<ChoiceButton> choices = new List<ChoiceButton>();

    public void Show(params string[] choices)
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);

            ClearAllCurrentChoices();

            ShowingChoices(choices);
        }
    }

    public void Hide()
    {
        if (gameObject.activeInHierarchy)
        {
            ClearAllCurrentChoices();

            gameObject.SetActive(false);
        }
    }

    private void ClearAllCurrentChoices()
    {
        foreach (ChoiceButton b in choices)
        {
            DestroyImmediate(b.gameObject);
        }
        choices.Clear();
    }

    public bool isWaitingForChoiceToBeMade { get { return !lastChoiceMade.hasBeenMade; } }
    public void ShowingChoices(string[] choices)
    {
        lastChoiceMade.Reset();

        for(int i = 0; i < choices.Length; i++)
        {
            createChoice(choices[i]);
        }
    }

    private void createChoice(string choice)
    {
        GameObject obj = Instantiate(choicePrefab, choicePrefab.transform.parent);
        obj.SetActive(true);

        ChoiceButton b = obj.GetComponent<ChoiceButton>();
        b.tmpro.text = choice;
        b.choiceIndex = choices.Count;

        choices.Add(b);
    }

    public CHOICE lastChoiceMade = new CHOICE();
    [System.Serializable]
    public class CHOICE
    {
        public bool hasBeenMade { get { return title != "" && index != -1; } }

        public string title = "";
        public int index = -1;

        public void Reset()
        {
            title = "";
            index = -1;
        }
    }

    public void MakeChoice(ChoiceButton button)
    {
        lastChoiceMade.index = button.choiceIndex;
        lastChoiceMade.title = button.tmpro.text;
    }
}
