using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterLineManager
{
    public static LINE Interpret(string rawLine)
    {
        return new LINE(rawLine);
    }

    public class LINE
    {
        public string speaker = "";
        public bool hiddenSpeaker = false;
        public bool thinkingSpeaker = false;
        public List<SEGMENT> segments = new List<SEGMENT>();
        public List<string> actions = new List<string>();
        public bool isDialogue = false;

        public LINE(string rawLine)
        {
            string[] dialogueAndActions = rawLine.Split('"');

            // The only reason I'm using this aux variable is so
            // the line of code doesn't take too much space on screen lol.
            string aux = dialogueAndActions.Length == 3 ? dialogueAndActions[2] : dialogueAndActions[0];
            string[] actionArr = aux.Split(' ');

            isDialogue = (dialogueAndActions.Length == 3);

            if (dialogueAndActions.Length == 3)
            {
                dialogueAndActions[1] = dialogueAndActions[1].Replace('#', '\n');

                speaker = dialogueAndActions[0] == "" ? NovelController.instance.cachedLastSpeaker : dialogueAndActions[0];
                hiddenSpeaker = (dialogueAndActions[0] == "") ? NovelController.instance.cachedHiddenSpeaker : false;
                thinkingSpeaker = (dialogueAndActions[0] == "") ? NovelController.instance.cachedThinkingSpeaker : false;

                if (speaker[speaker.Length - 1] == ' ')
                {
                    speaker = speaker.Remove(speaker.Length - 1);
                }

                if (speaker.EndsWith("(hidden)"))
                {
                    hiddenSpeaker = true;
                    speaker = speaker.Remove(speaker.Length - 8);
                }
                if(speaker.EndsWith("(thinking)"))
                {
                    thinkingSpeaker = true;
                    speaker = speaker.Remove(speaker.Length - 10);
                }

                NovelController.instance.cachedLastSpeaker = speaker;
                NovelController.instance.cachedHiddenSpeaker = hiddenSpeaker;
                NovelController.instance.cachedThinkingSpeaker = thinkingSpeaker;

                SegmentDialogue(dialogueAndActions[1]);
            }

            for (int i = 0; i < actionArr.Length; i++)
            {
                actions.Add(actionArr[i]);
            }
        }

        void SegmentDialogue(string dialogue)
        {
            segments.Clear();

            string[] parts = dialogue.Split('{','}');
            for(int i = 0; i < parts.Length; i++)
            {
                SEGMENT segment = new SEGMENT();

                bool isOdd = (i % 2 != 0);
                if (isOdd)
                {
                    string[] commandData = parts[i].Split(' ');
                    switch (commandData[0])
                    {
                        case "a":
                            segment.trigger = SEGMENT.TRIGGER.waitClick;
                            MyNewPretext(ref segment);
                            break;
                        case "w":
                            segment.trigger = SEGMENT.TRIGGER.autoDelay;
                            segment.autoDelayTime = float.Parse(commandData[1]);
                            MyNewPretext(ref segment);
                            break;
                    }
                    i++;
                }

                segment.myDialogue = parts[i];
                segment.myLine = this;

                segments.Add(segment);
            }
        }

        public void MyNewPretext(ref SEGMENT segment)
        {
            string pretext = "";
            for (int i = 0; i < segments.Count; i++)
            {
                string[] aux1;
                string aux2 = "";
                if (segments[i].myDialogue.Contains("["))
                {
                    aux1 = segments[i].myDialogue.Split('[', ']');
                    for (int j = 0; j < aux1.Length; j++)
                    {
                        if(j % 2 == 0)
                        {
                            aux2 += aux1[j];
                        }
                    }
                }
                else
                {
                    aux2 = segments[i].myDialogue;
                }
                pretext += aux2;
            }
            segment.myPretext = pretext;
        }

        public class SEGMENT
        {
            public LINE myLine;
            public string myDialogue;
            public string myPretext;
            public enum TRIGGER{waitClick, autoDelay}
            public TRIGGER trigger;

            public float autoDelayTime = 0;

            public void Run()
            {
                if (running != null)
                {
                    NovelController.instance.StopCoroutine(running);
                }
                running = NovelController.instance.StartCoroutine(Running());
            }

            public bool isRunning { get { return running != null; } }
            Coroutine running = null;
            public TextArchitect architect = null;
            private IEnumerator Running()
            {
                //TagManager.Inject(ref myDialogue);

                string[] parts = myDialogue.Split('[', ']');
                for (int i = 0; i < parts.Length; i++)
                {
                    bool isOdd = (i % 2 != 0);
                    if (isOdd)
                    {
                        DialogueEvents.HandleEvent(parts[i], this);
                        i++;
                    }

                    string targDialog = parts[i];

                    DialogueSystem.instance.SetSpeechPanel(myLine.speaker);
                    if (myLine.speaker == "Anon")
                    {
                        if(myPretext == "")
                        {
                            DialogueSystem.instance.SayOverride(targDialog, "", 5f);
                        }
                        else
                        {
                            DialogueSystem.instance.SayAdditive(targDialog, myPretext, "", 5f);
                        }
                    }
                    else if (myLine.speaker == "???")
                    {
                        if (myPretext == "")
                        {
                            DialogueSystem.instance.SayOverride(targDialog, "???", 5f);
                        }
                        else
                        {
                            DialogueSystem.instance.SayAdditive(targDialog, myPretext, "???", 5f);
                        }
                    }
                    else
                    {
                        Characters character = CharacterManager.instance.GetCharacter(myLine.speaker);
                        character.Say(targDialog, myPretext, !myLine.hiddenSpeaker, myLine.thinkingSpeaker);
                    }

                    architect = DialogueSystem.instance.currentArchitect;
                    while (architect.isConstructing)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }

                running = null;
            }
        }
    }
}
