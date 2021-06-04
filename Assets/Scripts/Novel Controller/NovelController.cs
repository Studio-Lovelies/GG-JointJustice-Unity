using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovelController : MonoBehaviour
{
    // SINGLETON DEFINITION -----------------------------------------------------------------------------------------------
    public static NovelController instance;
    void Awake()
    {
        instance = this;
    }

    bool _next = false;
    public void Next()
    {
        _next = true;
    }

    List<string> data = new List<string>();
    public void LoadChapterFile(string fileName)
    {
        data = FileManager.LoadFile(FileManager.savPath + "Resources/Story/" + fileName);
        cachedLastSpeaker = "";

        if (handlingChapterFile != null)
        {
            StopCoroutine(handlingChapterFile);
        }
        handlingChapterFile = StartCoroutine(HandlingChapterFile());

        Next();
    }

    private int chapterProgress = 0;
    private Dictionary<string, int> labels = new Dictionary<string, int>();
    Coroutine handlingChapterFile;
    private IEnumerator HandlingChapterFile()
    {
        foreach (string line in data)
        {
            if (line.StartsWith("LABEL"))
            {
                labels.Add(line, chapterProgress);
            }
            chapterProgress++;
        }

        chapterProgress = 0;

        while(chapterProgress < data.Count)
        {
            if (_next && isWaiting == null)
            {
                while (data[chapterProgress].StartsWith("//"))
                {
                    chapterProgress++;
                }

                string line = data[chapterProgress];
                if (line.StartsWith("CHOICE"))
                {
                    yield return HandlingChoiceLine();
                    chapterProgress++;
                }
                else
                {
                    HandleLine(line);
                    chapterProgress++;
                    while (isHandlingLine)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator HandlingChoiceLine()
    {
        List<string> choices = new List<string>();
        List<string> actions = new List<string>();

        while (true)
        {
            chapterProgress++;

            if (data[chapterProgress] == "{")
            {
                continue;
            }

            if (data[chapterProgress] != "}")
            {
                choices.Add(data[chapterProgress]);
                chapterProgress++;
                actions.Add(data[chapterProgress]);
            }
            else
            {
                break;
            }
        }

        ChoiceScreen.instance.Show(choices.ToArray());
        yield return new WaitForEndOfFrame();
        while (ChoiceScreen.instance.isWaitingForChoiceToBeMade)
        {
            yield return new WaitForEndOfFrame();
        }

        ChoiceScreen.instance.Hide();

        string action = actions[ChoiceScreen.instance.lastChoiceMade.index];
        HandleLine(action);

        while (isHandlingLine)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    /*
    public void NextLine()
    {
        if (isWaiting == null && !DialogueSystem.instance.isSpeaking)
        {
            progress++;
            while (data[progress].StartsWith("//"))
            {
                progress++;
            }
            HandleLine(data[progress]);
        }
    }
    */

    public void HandleLine(string rawLine)
    {
        ChapterLineManager.LINE line = ChapterLineManager.Interpret(rawLine);

        StopHandlingLine();
        handlingLine = StartCoroutine(HandlingLine(line));
    }

    void StopHandlingLine()
    {
        if (isHandlingLine)
        {
            StopCoroutine(handlingLine);
        }
        handlingLine = null;
    }

    public bool isHandlingLine { get { return handlingLine != null; } }
    Coroutine handlingLine = null;
    private IEnumerator HandlingLine(ChapterLineManager.LINE line)
    {
        _next = false;
        int lineProgress = 0;

        while (lineProgress < line.segments.Count)
        {
            _next = false;
            ChapterLineManager.LINE.SEGMENT segment = line.segments[lineProgress];

            if (lineProgress > 0)
            {
                if (segment.trigger == ChapterLineManager.LINE.SEGMENT.TRIGGER.autoDelay)
                {
                    for(float timer = segment.autoDelayTime; timer >= 0; timer -= Time.deltaTime)
                    {
                        yield return new WaitForEndOfFrame();
                        if (_next)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    while (!_next)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }
            }
            _next = false;

            segment.Run();

            while (segment.isRunning)
            {
                yield return new WaitForEndOfFrame();
                /*
                if (_next && !segment.architect.skip)
                {
                    segment.architect.skip = true;
                }
                */
            }

            lineProgress++;

            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < line.actions.Count; i++)
        {
            HandleAction(line.actions[i]);
        }
        string lastAction = line.actions[line.actions.Count - 1].Split('(',')')[0];
        string firstAction = line.actions[0].Split('(', ')')[0];
        if (!line.isDialogue && lastAction != "WAIT" && !actionsThatDontWait.Contains(firstAction))
        {
            isWaiting = StartCoroutine(Command_WAIT("0"));
        }
        handlingLine = null;
    }
    private List<string> actionsThatDontWait = new List<string>() { "SHOUT", "WIDESHOT", "GAVEL" };

    public string cachedLastSpeaker = "";
    public bool cachedHiddenSpeaker = false;
    public bool cachedThinkingSpeaker = false;

    private void HandleAction(string action)
    {
        string[] data = action.Split('(', ')');

        switch (data[0])
        {
            case "JUMP":
                Command_JUMP(data[1]);
                break;
            case "SET_COURT_RECORD":
                Command_SET_COURT_RECORD(data[1]);
                break;
            case "ADD_TO_COURT_RECORD":
                Command_ADD_TO_COURT_RECORD(data[1]);
                break;
            case "SHOUT":
                Command_SHOUT(data[1]);
                break;
            case "TESTIMONY_START":
                Command_TESTIMONY_START();
                break;
            case "TESTIMONY_END":
                Command_TESTIMONY_END();
                break;
            case "SET_CHARACTER_EMOTION":
                Command_SET_CHARACTER_EMOTION(data[1]);
                break;
            case "FADE_IN_CHARACTER":
                Command_FADE_IN_CHARACTER(data[1]);
                break;
            case "FADE_OUT_CHARACTER":
                Command_FADE_OUT_CHARACTER(data[1]);
                break;
            case "SET_LOCATION_CHARACTER":
                Command_SET_LOCATION_CHARACTER(data[1]);
                break;
            case "JUMPCUT":
                Command_JUMPCUT(data[1]);
                break;
            case "SET_FOREGROUND":
                Command_SET_FOREGROUND(data[1]);
                break;
            case "FADE_IN_BACKGROUND":
                Command_FADE_IN_BACKGROUND(data[1]);
                break;
            case "FADE_IN_FOREGROUND":
                Command_FADE_IN_FOREGROUND(data[1]);
                break;
            case "MOVE_BACKGROUND":
                Command_MOVE_BACKGROUND(data[1]);
                break;
            case "MOVE_FOREGROUND":
                Command_MOVE_FOREGROUND(data[1]);
                break;
            case "FADE_TO_BLACK":
                Command_FADE_TO_BLACK(data[1]);
                break;
            case "SCREEN_SHAKE":
                Command_SCREEN_SHAKE(data[1]);
                break;
            case "SHOW_EVIDENCE_LEFT":
                Command_SHOW_EVIDENCE_LEFT(data[1]);
                break;
            case "SHOW_EVIDENCE_RIGHT":
                Command_SHOW_EVIDENCE_RIGHT(data[1]);
                break;
            case "REMOVE_EVIDENCE_LEFT":
                Command_REMOVE_EVIDENCE_LEFT(data[1]);
                break;
            case "REMOVE_EVIDENCE_RIGHT":
                Command_REMOVE_EVIDENCE_RIGHT(data[1]);
                break;
            case "WIDESHOT":
                Command_WIDESHOT(data[1]);
                break;
            case "GAVEL":
                Command_GAVEL(data[1]);
                break;
            case "COURT_PAN":
                Command_COURT_PAN(data[1]);
                break;
            case "WAIT":
                isWaiting = StartCoroutine(Command_WAIT(data[1]));
                break;
            case "PLAY_SFX":
                Command_PLAY_SFX(data[1]);
                break;
            case "PLAY_MUSIC":
                Command_PLAY_MUSIC(data[1]);
                break;
            case "STOP_MUSIC":
                Command_STOP_MUSIC();
                break;
            case "FADE_MUSIC":
                Command_FADE_MUSIC(data[1]);
                break;
        }
    }

    private void Command_JUMP(string data)
    {
        labels.TryGetValue("LABEL " + data, out chapterProgress);
    }

    private void Command_SET_COURT_RECORD(string data)
    {
        string[] aux1 = data.Split(',');
        List<string> aux2 = new List<string>();
        for (int i = 0; i < aux1.Length; i++)
        {
            aux2.Add(aux1[i]);
        }
        EvidenceManager.instance.SetCourtRecord(aux2);
    }

    private void Command_ADD_TO_COURT_RECORD(string data)
    {
        EvidenceManager.instance.AddToCourtRecord(data);
    }

    private void Command_SHOUT(string data)
    {
        string[] parameters = data.Split(',');

        AudioManager.instance.PlaySFX(parameters[1] + parameters[0], 1f);
        StartCoroutine(BackgroundForegroundController.instance.Shout(parameters[1]));
        isWaiting = StartCoroutine(Command_WAIT("1.33"));
    }

    private void Command_TESTIMONY_START()
    {
        AudioManager.instance.PlaySFX("testimony", 1f);
        BackgroundForegroundController.instance.ActivateTestimony();
    }

    private void Command_TESTIMONY_END()
    {
        BackgroundForegroundController.instance.DeactivateTestimony();
    }

    private void Command_SET_CHARACTER_EMOTION(string data)
    {
        string[] parameters = data.Split(',');

        parameters[0] = parameters[0].Replace("_"," ");
        Characters chara = CharacterManager.instance.GetCharacter(parameters[0]);

        if (parameters.Length > 2 && parameters[2] == "newActive")
        {
            CharacterManager.instance.SetActiveCharacter(chara);
        }

        if (parameters[1].EndsWith("_Ani"))
        {
            chara.ChangeEmotion(parameters[1]);
        }
        else
        {
            chara.ChangeEmotion(parameters[1] + "_Idle");
        }
    }

    private void Command_FADE_IN_CHARACTER(string data)
    {
        string[] parameters = data.Split(',');
        parameters[0] = parameters[0].Replace("_", " ");
        float fval = float.Parse(parameters[2]);
        StartCoroutine(CharacterManager.instance.GetCharacter(parameters[0]).FadeIn(parameters[1], fval));
    }

    private void Command_FADE_OUT_CHARACTER(string data)
    {
        if (CharacterManager.instance.activeCharacter != null)
        {
            StartCoroutine(CharacterManager.instance.activeCharacter.FadeOut(float.Parse(data)));
        }
    }

    private void Command_SET_LOCATION_CHARACTER(string data)
    {
        string[] parameters = data.Split(',');
        parameters[1] = parameters[1].Replace('_', ' ');
        CharacterManager.instance.SetLocationCharacter(parameters[0], parameters[1]);
    }

    private void Command_JUMPCUT(string data)
    {
        switch (data)
        {
            case "courtJudge":
                BackgroundForegroundController.instance.SetBackgroundImage(data, 0f);
                BackgroundForegroundController.instance.SetForegroundImage("", 0f);
                break;
            case "courtDefense":
                BackgroundForegroundController.instance.SetBackgroundImage(data, 0f);
                BackgroundForegroundController.instance.SetForegroundImage(data + "2", 0f);
                break;
            case "courtProsecution":
                BackgroundForegroundController.instance.SetBackgroundImage(data, 0f);
                BackgroundForegroundController.instance.SetForegroundImage(data + "2", 0f);
                break;
            case "courtWitness":
                BackgroundForegroundController.instance.SetBackgroundImage(data, 0f);
                BackgroundForegroundController.instance.SetForegroundImage(data + "2", 0f);
                break;
            case "courtAssistant":
                BackgroundForegroundController.instance.SetBackgroundImage(data, 0f);
                BackgroundForegroundController.instance.SetForegroundImage("", 0f);
                break;
        }
    }

    private void Command_SET_FOREGROUND(string data)
    {
        BackgroundForegroundController.instance.SetForegroundImage(data, 0f);
    }

    private void Command_FADE_IN_BACKGROUND(string data)
    {
        string[] parameters = data.Split(',');
        float fval = float.Parse(parameters[1]);
        BackgroundForegroundController.instance.SetBackgroundImage(parameters[0], fval);
    }

    private void Command_FADE_IN_FOREGROUND(string data)
    {
        string[] parameters = data.Split(',');
        float fval = float.Parse(parameters[1]);
        BackgroundForegroundController.instance.SetForegroundImage(parameters[0], fval);
    }

    private void Command_MOVE_BACKGROUND(string data)
    {
        string[] parameters = data.Split(',');
        float fval1 = float.Parse(parameters[0]);
        float fval2 = float.Parse(parameters[1]);
        float fval3 = float.Parse(parameters[2]);
        BackgroundForegroundController.instance.MoveBackgroundImage(fval1, fval2, fval3);
    }

    private void Command_MOVE_FOREGROUND(string data)
    {
        string[] parameters = data.Split(',');
        float fval1 = float.Parse(parameters[0]);
        float fval2 = float.Parse(parameters[1]);
        float fval3 = float.Parse(parameters[2]);
        BackgroundForegroundController.instance.MoveForegroundImage(fval1, fval2, fval3);
    }

    private void Command_FADE_TO_BLACK(string data)
    {
        float fval = float.Parse(data);
        BackgroundForegroundController.instance.FadeOutBackground(fval);
        BackgroundForegroundController.instance.FadeOutForeground(fval);
    }

    private void Command_SCREEN_SHAKE(string data)
    {
        StartCoroutine(BackgroundForegroundController.instance.ShakeScreen(float.Parse(data)));
    }

    private void Command_SHOW_EVIDENCE_LEFT(string data)
    {
        BackgroundForegroundController.instance.ShowEvidence(data, true);
    }

    private void Command_SHOW_EVIDENCE_RIGHT(string data)
    {
        BackgroundForegroundController.instance.ShowEvidence(data, false);
    }

    private void Command_REMOVE_EVIDENCE_LEFT(string data)
    {
        StartCoroutine(BackgroundForegroundController.instance.RemoveEvidence(true));
    }

    private void Command_REMOVE_EVIDENCE_RIGHT(string data)
    {
        StartCoroutine(BackgroundForegroundController.instance.RemoveEvidence(false));
    }

    private void Command_WIDESHOT(string data)
    {
        string[] parameters = data.Split(',');
        BackgroundForegroundController.instance.Wideshot(parameters);
        isWaiting = StartCoroutine(Command_WAIT("2.41"));
    }

    private void Command_GAVEL(string data)
    {
        DialogueSystem.instance.Close();
        BackgroundForegroundController.instance.SetBackgroundImage("blackScreen", 0f);
        BackgroundForegroundController.instance.Gavel(int.Parse(data));
        if (data == "1")
        {
            isWaiting = StartCoroutine(Command_WAIT("0.666"));
        }
        else
        {
            isWaiting = StartCoroutine(Command_WAIT("1.333"));
        }
    }

    private void Command_COURT_PAN(string data)
    {
        string[] parameters = data.Split(',');
        StartCoroutine(BackgroundForegroundController.instance.CourtPan(parameters[0], parameters[1]));
    }

    private Coroutine isWaiting = null;
    private IEnumerator Command_WAIT(string data)
    {
        float fval = 0;
        float.TryParse(data, out fval);
        yield return new WaitForSecondsRealtime(fval);
        Next();
        stopWaiting();
    }
    private void stopWaiting()
    {
        StopCoroutine(isWaiting);
        isWaiting = null;
    }

    private void Command_PLAY_SFX(string data)
    {
        string sfxName;
        float volume = 1f;

        if (data.Contains(","))
        {
            string[] parameters = data.Split(',');
            sfxName = parameters[0];
            float.TryParse(parameters[1], out volume);
        }
        else
        {
            sfxName = data;
        }

        AudioManager.instance.PlaySFX(sfxName, volume);
    }

    private void Command_PLAY_MUSIC(string data)
    {
        string songName;
        float volume = 1f;
        bool loop = true;

        if (data.Contains(","))
        {
            string[] parameters = data.Split(',');
            songName = parameters[0];
            foreach(string p in parameters)
            {
                float fval = 0;
                bool bval = false;
                if (float.TryParse(p, out fval))
                {
                    volume = fval;
                    continue;
                }
                if (bool.TryParse(p, out bval))
                {
                    loop = bval;
                    continue;
                }
            }
        }
        else
        {
            songName = data;
        }

        AudioManager.instance.PlaySong(songName, volume, loop);
    }

    private void Command_STOP_MUSIC()
    {
        AudioManager.instance.Stop();
    }

    private void Command_FADE_MUSIC(string data)
    {
        AudioManager.instance.FadeOutActiveSong(float.Parse(data));
    }
}
