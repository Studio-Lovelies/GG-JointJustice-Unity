using System;
using System.Collections;
using System.Linq;
using SaveFiles;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SaveFileWatcher : MonoBehaviour
{
    private readonly SaveFiles.Proxy _saveFileProxy = new SaveFiles.Proxy();
    private Text _text;

    // Start is called before the first frame update
    private void Start()
    {
        // make sure a saveFile exists
        _saveFileProxy.UpdateCurrentSaveData((ref SaveData _) => {});

        _text = GetComponent<Text>();
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        var currentSave = _saveFileProxy.Load();

        var chapters = (SaveFiles.SaveData.Progression.Chapters[])Enum.GetValues(typeof(SaveFiles.SaveData.Progression.Chapters));
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
