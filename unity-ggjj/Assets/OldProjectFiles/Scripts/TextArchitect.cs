using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextArchitect {
    /// <summary>
    /// A dictionary keeping tabs on all architects present in a scene. Prevents multiple architects from influencing the same text object simultaneously.
    /// </summary>
    private static Dictionary<TextMeshProUGUI, TextArchitect> activeArchitects = new Dictionary<TextMeshProUGUI, TextArchitect>();

    private string preText;
    private string targetText;
    public int charactersPerFrame = 1;
    public float speed = 1f;
    public bool skip = false;

    public bool isConstructing { get { return buildProcess != null; } }
    Coroutine buildProcess = null;
    TextMeshProUGUI tmpro;

    public TextArchitect(TextMeshProUGUI tmpro, string targetText, string preText = "", int charactersPerFrame = 1, float speed = 1f)
    {
        this.tmpro = tmpro;
        this.targetText = targetText;
        this.preText = preText;
        this.charactersPerFrame = charactersPerFrame;
        this.speed = Mathf.Clamp(speed, 1f, 300f);

        Initiate();
    }

    public void Stop()
    {
        if (isConstructing)
        {
            DialogueSystem.instance.StopCoroutine(buildProcess);
        }
        buildProcess = null;
    }

    IEnumerator Construction()
    {
        int runsThisFrame = 0;

        tmpro.text = "";
        tmpro.text += preText;

        tmpro.ForceMeshUpdate();
        TMP_TextInfo inf = tmpro.textInfo;
        int vis = inf.characterCount;

        tmpro.text += targetText;

        tmpro.ForceMeshUpdate();
        inf = tmpro.textInfo;
        int max = inf.characterCount;

        tmpro.maxVisibleCharacters = vis;

        while (vis < max)
        {
            //allow skipping by increasing the characters per frame and the speed of occurance.
            /*
            if (skip)
            {
                speed = 1;
                charactersPerFrame = charactersPerFrame < 5 ? 5 : charactersPerFrame + 3;
            }
            */
            //reveal a certain number of characters per frame.
            while (runsThisFrame < charactersPerFrame)
            {
                vis++;
                tmpro.maxVisibleCharacters = vis;
                runsThisFrame++;
                AudioManager.instance.PlaySFX("maleTalk", 0.125f);
            }
            //wait for the next available revelation time.
            runsThisFrame = 0;
            yield return new WaitForSecondsRealtime(0.01f * speed);
        }
        //terminate the architect and remove it from the active log of architects.
        Terminate();
    }

    void Initiate()
    {
        //check if an architect for this text object is already running. if it is, terminate it. Do not allow more than one architect to affect the same text object at once.
        TextArchitect existingArchitect = null;
        if (activeArchitects.TryGetValue(tmpro, out existingArchitect))
        {
            existingArchitect.Terminate();
        }
        buildProcess = DialogueSystem.instance.StartCoroutine(Construction());
        activeArchitects.Add(tmpro, this);
    }

    /// <summary>
    /// Terminate this architect. Stops the text generation process and removes it from the cache of all active architects.
    /// </summary>
    public void Terminate()
    {
        activeArchitects.Remove(tmpro);
        if (isConstructing)
        {
            DialogueSystem.instance.StopCoroutine(buildProcess);
        }
        buildProcess = null;
    }

    public void Renew(string targ, string pre)
    {
        targetText = targ;
        preText = pre;

        skip = false;

        if (isConstructing)
        {
            DialogueSystem.instance.StopCoroutine(buildProcess);
        }
        buildProcess = DialogueSystem.instance.StartCoroutine(Construction());
    }
}