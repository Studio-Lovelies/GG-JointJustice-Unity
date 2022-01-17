using System;
using System.Collections;
using System.Linq;
using Savefiles;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SaveFileWatcher : MonoBehaviour
{
    private readonly Savefiles.Proxy _savefileProxy = new Savefiles.Proxy();
    private Text _text;

    // Start is called before the first frame update
    private void Start()
    {
        // make sure a savefile exists
        _savefileProxy.UpdateCurrentSaveData((ref SaveData _) => {});

        _text = GetComponent<Text>();
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        var currentSave = _savefileProxy.Load();

        var chapters = (Savefiles.SaveData.Progression.Chapters[])Enum.GetValues(typeof(Savefiles.SaveData.Progression.Chapters));
        string currentState = string.Join("\n", chapters.Select(chapter => $"{chapter}: {currentSave.GameProgression.UnlockedChapters.HasFlag(chapter)}"));

        _text.text = "Checking for new state in 3.\n\n" + currentState;
        yield return new WaitForSeconds(1.0f);
        _text.text = "Checking for new state in 2..\n\n" + currentState;
        yield return new WaitForSeconds(1.0f);
        _text.text = "Checking for new state in 1..\n\n" + currentState;
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Loop());
    }
}
