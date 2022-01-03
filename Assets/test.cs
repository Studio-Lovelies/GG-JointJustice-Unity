using Ink.Runtime;
using UnityEngine;

public class test : MonoBehaviour
{
    public TextAsset text;

    // Start is called before the first frame update
    void Start()
    {
        var scriptReader = new ScriptReader(new Story(text.text));
    }
}
