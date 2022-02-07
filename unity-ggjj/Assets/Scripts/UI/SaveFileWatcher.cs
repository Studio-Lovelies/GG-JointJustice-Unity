using System;
using System.Collections;
using System.Linq;
using SaveFiles;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SaveFileWatcher : MonoBehaviour
{
    private Text _text;

    // Start is called before the first frame update
    private void Start()
    {
        PlayerPrefsProxy.EnsureSaveDataExists();

        _text = GetComponent<Text>();
        StartCoroutine(Loop());
    }

    /// <summary>
    /// Loads the currently save data, displays which chapters are currently unlocked, waits for 3 seconds and restarts
    /// </summary>
    /// <returns>An enumerator to run this as a Coroutine</returns>
    private IEnumerator Loop()
    {
        var currentSave = PlayerPrefsProxy.Load();

        var chapters = (SaveData.Progression.Chapters[])Enum.GetValues(typeof(SaveData.Progression.Chapters));
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
