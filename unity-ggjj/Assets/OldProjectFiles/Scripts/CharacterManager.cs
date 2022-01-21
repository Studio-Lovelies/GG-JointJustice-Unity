using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    // SINGLETON DEFINITION -----------------------------------------------------------------------------------------------
    public static CharacterManager instance;
    void Awake()
    {
        instance = this;
        int i = 0;
        foreach(GameObject obj in characterObjects)
        {
            characterDictionary.Add(obj.GetComponent<Characters>().characterName, i);
            i++;
        }
    }

    // MY VARIABLES -------------------------------------------------------------------------------------------------------
    public RectTransform characterPanel;

    public Characters activeCharacter = null;
    // The list "characters" here has all the characters on scene.
    private List<Characters> characters = new List<Characters>();
    // The dictionary "characterDictionary" has an index for every character on scene.
    public Dictionary<string, int> characterDictionary = new Dictionary<string, int>();
    // The list "listLocationCharacter" has a list of all Location-Character relationships.
    public List<LOCATION_CHARACTER> listLocationCharacter = new List<LOCATION_CHARACTER>();

    public GameObject[] characterObjects;

    // FUNCTIONS ----------------------------------------------------------------------------------------------------------
    // Our friendly function "GetCharacter" here returns the character you want from the list of characters, or creates it
    // if it didn't exist.
    // The bool "enableCharacter" determines whether the character is enabled at start or not.
    public Characters GetCharacter(string characterName, bool enableCharacter = false)
    {
        Characters characterToGet = characters.Find(x => x.characterName == characterName);
        if (characterToGet != null)
        {
            return characterToGet;
        }
        else
        {
            // Be sure to pass the correct name and to name every folder correctly!~
            GameObject ob = Instantiate(characterObjects[characterDictionary[characterName]], characterPanel);

            // Adds the new character to the list and dictionary.
            characters.Add(ob.GetComponent<Characters>());

            ob.SetActive(enableCharacter);

            return ob.GetComponent<Characters>();
        }
    }

    public void SetActiveCharacter(Characters newActiveCharacter)
    {
        if (activeCharacter != null)
        {
            activeCharacter.gameObject.SetActive(false);
        }
        newActiveCharacter.gameObject.SetActive(true);
        activeCharacter = newActiveCharacter;

        switch (GetLocationWCharacter(newActiveCharacter))
        {
            case "courtJudge":
                BackgroundForegroundController.instance.SetBackgroundImage("courtJudge", 0f);
                BackgroundForegroundController.instance.SetForegroundImage("", 0f);
                break;
            case "courtDefense":
                BackgroundForegroundController.instance.SetBackgroundImage("courtDefense", 0f);
                BackgroundForegroundController.instance.SetForegroundImage("courtDefense2", 0f);
                break;
            case "courtProsecution":
                BackgroundForegroundController.instance.SetBackgroundImage("courtProsecution", 0f);
                BackgroundForegroundController.instance.SetForegroundImage("courtProsecution2", 0f);
                break;
            case "courtWitness":
                BackgroundForegroundController.instance.SetBackgroundImage("courtWitness", 0f);
                BackgroundForegroundController.instance.SetForegroundImage("courtWitness2", 0f);
                break;
            case "courtAssistant":
                BackgroundForegroundController.instance.SetBackgroundImage("courtAssistant", 0f);
                BackgroundForegroundController.instance.SetForegroundImage("", 0f);
                break;
        }
    }

    public void SetLocationCharacter(string location, string character)
    {
        LOCATION_CHARACTER a;
        a.location = location;
        a.character = GetCharacter(character);

        listLocationCharacter.Add(a);

        // TO-DO: Make this function find and remove previous location-character relationship if found.
    }

    public string GetLocationWCharacter(Characters character)
    {
        return listLocationCharacter.Find(x => x.character == character).location;
    }

    public Characters GetCharacterWLocation(string location)
    {
        return listLocationCharacter.Find(x => x.location == location).character;
    }
}

public struct LOCATION_CHARACTER
{
    public string location;
    public Characters character;
}