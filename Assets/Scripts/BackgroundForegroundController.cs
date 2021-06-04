using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundForegroundController : MonoBehaviour
{
    public static BackgroundForegroundController instance;
    void Awake()
    {
        instance = this;
    }

    public Image background;
    public Image foreground;

    public GameObject screenShakeObject;
    public GameObject courtPanObject;
    private GameObject evidenceShown;
    public GameObject wideshotTemplate;
    public GameObject wideshotScene;
    public GameObject gavelScreen;
    public GameObject shoutObject;
    public GameObject testimonyObject;

    public List<Sprite> backgroundSprites;
    public List<Sprite> foregroundSprites;
    public List<Sprite> wideshotCharacterSprites;
    public List<Sprite> shoutSprites;

    public void SetBackgroundImage(string newActiveImageName, float time)
    {
        StartCoroutine(SetImage(background, newActiveImageName, time));
    }
    public void SetForegroundImage(string newActiveImageName, float time)
    {
        StartCoroutine(SetImage(foreground, newActiveImageName, time));
    }

    public void MoveBackgroundImage(float posX, float posY, float time = 0f)
    {
        if (posX != 0)
        {
            StartCoroutine(MoveImageHorizontally(background, posX, time, true));
        }
        else if (posY != 0)
        {
            StartCoroutine(MoveImageVertically(background, posY, time, true));
        }
    }
    public void MoveForegroundImage(float posX, float posY, float time = 0f)
    {
        if (posX != 0)
        {
            StartCoroutine(MoveImageHorizontally(foreground, posX, time, true));
        }
        else if (posY != 0)
        {
            StartCoroutine(MoveImageVertically(foreground, posY, time, true));
        }
    }

    public void FadeOutBackground(float time)
    {
        StartCoroutine(FadeOutImage(background, time));
    }
    public void FadeOutForeground(float time)
    {
        StartCoroutine(FadeOutImage(foreground, time));
    }

    private IEnumerator SetImage(Image image, string newActiveImageName, float time)
    {
        bool empty = false;
        if (image == background)
        {
            image.sprite = backgroundSprites.Find(x => x.name == newActiveImageName);
        }
        else
        {
            if (newActiveImageName != "")
            {
                image.sprite = foregroundSprites.Find(x => x.name == newActiveImageName);
            }
            else
            {
                empty = true;
            }
        }
        image.SetNativeSize();

        if (!empty)
        {
            if (time > 0)
            {
                image.color = new Color(0f, 0f, 0f, 1f);
                Color aux = new Color(0.05f, 0.05f, 0.05f, 1f);
                while (image.color.r < 1)
                {
                    image.color += aux;
                    yield return new WaitForSecondsRealtime(time / 20);
                }
            }
            else
            {
                image.color = new Color(1f, 1f, 1f, 1f);
            }
        }
        else
        {
            image.color = new Color(1f, 1f, 1f, 0f);
        }
    }

    public IEnumerator MoveImageHorizontally(Image image, float posX, float time, bool slow)
    {
        float newPosX = image.rectTransform.localPosition.x + posX;
        if (time > 0)
        {
            if (posX > 0)
            {
                while (image.rectTransform.localPosition.x < newPosX)
                {
                    image.rectTransform.localPosition += new Vector3(posX / (slow ? 100 : 20), 0, 0);
                    yield return new WaitForSecondsRealtime(time / (slow ? 100 : 20));
                }
            }
            else
            {
                while (image.rectTransform.localPosition.x > newPosX)
                {
                    image.rectTransform.localPosition += new Vector3(posX / (slow ? 100 : 20), 0, 0);
                    yield return new WaitForSecondsRealtime(time / (slow ? 100 : 20));
                }
            }
            image.rectTransform.localPosition = new Vector3(newPosX, image.rectTransform.localPosition.y, 0);
        }
        else
        {
            image.rectTransform.localPosition += new Vector3(posX, 0, 0);
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator MoveImageVertically(Image image, float posY, float time, bool slow)
    {
        float newPosY = image.rectTransform.localPosition.y + posY;
        if (time > 0)
        {
            if (posY > 0)
            {
                while (image.rectTransform.localPosition.y < newPosY)
                {
                    image.rectTransform.localPosition += new Vector3(0, posY / (slow ? 100 : 20), 0);
                    yield return new WaitForSecondsRealtime(time / (slow ? 100 : 20));
                }
            }
            else
            {
                while (image.rectTransform.localPosition.y > newPosY)
                {
                    image.rectTransform.localPosition += new Vector3(0, posY / (slow ? 100 : 20), 0);
                    yield return new WaitForSecondsRealtime(time / (slow ? 100 : 20));
                }
            }
            image.rectTransform.localPosition = new Vector3(image.rectTransform.localPosition.x, newPosY, 0);
        }
        else
        {
            image.rectTransform.localPosition += new Vector3(0, posY, 0);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FadeOutImage(Image image, float time)
    {
        if (time > 0)
        {
            //image.color = new Color(1f, 1f, 1f, 1f);
            Color aux = new Color(0.05f, 0.05f, 0.05f, 0f);
            while (image.color.r > 0)
            {
                image.color -= aux;
                yield return new WaitForSecondsRealtime(time / 20);
            }
        }
        else
        {
            image.color = new Color(0f, 0f, 0f, 1f);
        }
    }

    public IEnumerator ShakeScreen(float time)
    {
        Vector2 aux = new Vector3(640f, 384f);
        float timer = 0;
        while(timer < time)
        {
            screenShakeObject.GetComponent<RectTransform>().position = Random.insideUnitCircle * 40f + aux;

            yield return new WaitForEndOfFrame();

            timer += Time.deltaTime;
        }
        screenShakeObject.GetComponent<RectTransform>().position = Vector2.zero + aux;
    }

    public void ShowEvidence(string evidence, bool left)
    {
        Vector2 aux = new Vector3(640f, 384f);

        evidenceShown = Instantiate(EvidenceManager.instance.GetEvidence(evidence).gameObject, screenShakeObject.transform);
        evidenceShown.transform.position = new Vector3((left ? -800 : 800) + aux.x, 128 + aux.y, 0);

        StartCoroutine(MoveImageHorizontally(evidenceShown.GetComponent<Image>(), (left ? 400 : -400), 0.001f, false));
    }

    public IEnumerator RemoveEvidence(bool left)
    {
        Coroutine coroutine = null;
        coroutine = StartCoroutine(MoveImageHorizontally(evidenceShown.GetComponent<Image>(), (left ? -400 : 400), 0.001f, false));

        yield return new WaitForSecondsRealtime(1f);

        Destroy(evidenceShown);
    }

    public void Wideshot(string[] characters)
    {
        GameObject scene = Instantiate(wideshotScene, screenShakeObject.transform);
        for (int i = 0; i < characters.Length; i++)
        {
            foreach (Sprite s in wideshotCharacterSprites)
            {
                if (s.name == characters[i] + "_wideshot")
                {
                    Instantiate(wideshotTemplate, scene.transform).GetComponentInChildren<Image>().sprite = s;
                }
            }
        }

        AudioManager.instance.PlaySFX("mutter", 0.5f);

        Destroy(scene, 2.44f);
    }

    public void Gavel(int times)
    {
        GameObject scene = Instantiate(gavelScreen, screenShakeObject.transform);
        scene.GetComponent<Animator>().SetBool(((times == 1) ? "GavelOnce" : "GavelThrice"), true);
    }

    public IEnumerator Shout(string type)
    {
        shoutObject.SetActive(true);
        shoutObject.GetComponent<Image>().sprite = shoutSprites.Find(x => x.name == type);

        Vector2 aux = new Vector3(640f, 384f);
        float timer = 0;
        while (timer < 1.329f)
        {
            shoutObject.GetComponent<RectTransform>().position = Random.insideUnitCircle * 40f + aux;

            yield return new WaitForSecondsRealtime(0.04f);

            timer += 0.04f;
        }
        shoutObject.GetComponent<RectTransform>().position = Vector2.zero + aux;
        shoutObject.SetActive(false);
    }

    private GameObject activeTestimony;
    public void ActivateTestimony()
    {
        activeTestimony = Instantiate(testimonyObject, screenShakeObject.transform);
    }

    public void DeactivateTestimony()
    {
        DestroyImmediate(activeTestimony);
    }

    public IEnumerator CourtPan(string currentPos, string targetPos)
    {
        float currentPosX = 0f;
        float targetPosX = 0f;
        string targetBackground = "";
        string targetForeground = "";

        courtPanObject.transform.position = new Vector3(640, 384, 0);

        SetBackgroundImage("courtPan", 0f);
        SetForegroundImage("courtPan2", 0f);

        switch (currentPos)
        {
            case "LEFT":
                currentPosX = 0f;
                break;
            case "CENTER":
                currentPosX = -1312f;
                break;
            case "RIGHT":
                currentPosX = -3904f;
                break;
        }
        switch (targetPos)
        {
            case "LEFT":
                targetBackground = "courtDefense";
                targetForeground = "courtDefense2";
                targetPosX = 0f;
                break;
            case "CENTER":
                targetBackground = "courtWitness";
                targetForeground = "courtWitness2";
                targetPosX = -1312f;
                break;
            case "RIGHT":
                targetBackground = "courtProsecution";
                targetForeground = "courtProsecution2";
                targetPosX = -3904f;
                break;
        }

        if (targetPosX > currentPosX)
        {
            while (courtPanObject.transform.position.x < targetPosX)
            {
                courtPanObject.transform.position += new Vector3((targetPosX - currentPosX) / 20, 0, 0);
                yield return new WaitForSecondsRealtime(1 / 20);
            }
        }
        else
        {
            while (courtPanObject.transform.position.x > targetPosX)
            {
                courtPanObject.transform.position += new Vector3((targetPosX - currentPosX) / 20, 0, 0);
                yield return new WaitForSecondsRealtime(1 / 20);
            }
        }

        SetBackgroundImage(targetBackground, 0f);
        SetForegroundImage(targetForeground, 0f);

        courtPanObject.transform.position = new Vector3(640, 384, 0);
    }
}
