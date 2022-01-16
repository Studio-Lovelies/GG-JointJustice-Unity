using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Characters : MonoBehaviour
{
    public string characterName;

    // SAYING STUFF -------------------------------------------------------------------------------------------------------
    // The bool "add" determines whether the text is additive or not. Additive text keeps the previous text on screen.
    // Override (the other option) removes all the previous text on screen.
    public void Say(string text, string addText, bool newActive, bool thinking)
    {
        if (newActive)
        {
            CharacterManager.instance.SetActiveCharacter(this);
        }

        if (!thinking)
        {
            ChangeToTalking();
        }

        if (addText == "") DialogueSystem.instance.SayOverride(text, characterName, 5f);
        else DialogueSystem.instance.SayAdditive(text, addText, characterName, 5f);
    }

    // SPRITES AND THE SUCH -----------------------------------------------------------------------------------------------
    // Current emotion starts with "Normal_Idle" because that's how it starts in the animator panel.
    private string currentEmotion = "Normal_Idle";
    public string CurrentEmotion { get { return currentEmotion; } }

    public void ChangeEmotion(string emotion)
    {
        GetComponent<Animator>().SetBool(currentEmotion, false);
        // This is my hard-cody way of dealing with Unity's shitty animation system :)
        // Essentially, by assigning "Start_Animation" to true here (all animations need it in true to start), then
        // assingning it to false with an animation event (see StopRepeatingAnimation() below), we make sure the animation
        // can't start over again. This is because if "Start_Animation" and the bool for the animation to be played are
        // both true, then the animation will start itself again over and over (because it goes from any state (itself) to
        // any other state (itself, again)).
        GetComponent<Animator>().SetBool("Start_Animation", true);
        GetComponent<Animator>().SetBool(emotion, true);
        currentEmotion = emotion;
    }
    // See above for why this is necessary.
    public void StopRepeatingAnimation()
    {
        GetComponent<Animator>().SetBool("Start_Animation", false);
    }

    // This thing grabs the first part of the current emotion (say, 'normal' or 'angry') and concatenates it with
    // "_talking" to create a new emotion for the character. Same logic with the function below.
    public void ChangeToTalking()
    {
        if (CharacterManager.instance.activeCharacter == this)
        {
            string talkEmotion = currentEmotion.Split('_')[0] + "_Talking";
            ChangeEmotion(talkEmotion);
        }
    }
    public void ChangeToIdle()
    {
        if (CharacterManager.instance.activeCharacter == this)
        {
            string idleEmotion = currentEmotion.Split('_')[0] + "_Idle";
            ChangeEmotion(idleEmotion);
        }
    }

    public IEnumerator FadeIn(string emotion, float time)
    {
        CharacterManager.instance.SetActiveCharacter(this);
        ChangeEmotion(emotion + "_Idle");

        if (time > 0)
        {
            gameObject.GetComponentInChildren<Image>().color = new Color(1f, 1f, 1f, 0f);
            Color aux = new Color(0f, 0f, 0f, 0.05f);
            while (gameObject.GetComponentInChildren<Image>().color.a < 1)
            {
                gameObject.GetComponentInChildren<Image>().color += aux;
                yield return new WaitForSecondsRealtime(time / 20);
            }
        }
        else
        {
            gameObject.GetComponentInChildren<Image>().color = new Color(1f, 1f, 1f, 1f);
        }
    }

    public IEnumerator FadeOut(float time)
    {
        if (time > 0)
        {
            gameObject.GetComponentInChildren<Image>().color = new Color(1f, 1f, 1f, 1f);
            Color aux = new Color(0f, 0f, 0f, -0.05f);
            while (gameObject.GetComponentInChildren<Image>().color.a > 0)
            {
                gameObject.GetComponentInChildren<Image>().color += aux;
                yield return new WaitForSecondsRealtime(time / 20);
            }
        }
        else
        {
            gameObject.GetComponentInChildren<Image>().color = new Color(1f, 1f, 1f, 0f);
        }
    }
}