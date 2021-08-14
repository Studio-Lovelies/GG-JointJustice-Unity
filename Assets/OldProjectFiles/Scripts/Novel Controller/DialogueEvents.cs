using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueEvents
{
    public static void HandleEvent(string _event, ChapterLineManager.LINE.SEGMENT segment)
    {
        string[] eventData = _event.Split(' ');

        switch (eventData[0])
        {
            case "TEXTSPEED":
                Event_TEXTSPEED(eventData[1], segment);
                break;
        }   
    }

    static void Event_TEXTSPEED(string data, ChapterLineManager.LINE.SEGMENT seg)
    {
        string[] parts = data.Split(',');
        float speed = float.Parse(parts[0]);
        int charactersPerFrame = int.Parse(parts[1]);

        seg.architect.speed = speed;
        seg.architect.charactersPerFrame = charactersPerFrame;
    }
}
