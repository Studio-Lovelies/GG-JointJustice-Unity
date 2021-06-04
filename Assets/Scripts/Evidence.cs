using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Evidence : MonoBehaviour
{
    public string evidenceName;
    public string evidenceDescription;
    public Sprite evidenceSprite;
    public bool isProfile;

    public void Initialize(string name, string description, Sprite sprite, bool profile)
    {
        evidenceName = name;
        evidenceDescription = description;
        evidenceSprite = sprite;
        GetComponent<Image>().sprite = sprite;
        isProfile = profile;
    }
}
