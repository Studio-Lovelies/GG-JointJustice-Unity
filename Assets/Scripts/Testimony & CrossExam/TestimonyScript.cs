using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestimonyScript : MonoBehaviour
{
    public GameObject testimonyLogo;

    public void ActivateTestimonyLogo()
    {
        GetComponent<Animator>().SetBool("TestimonyEnd", true);
        testimonyLogo.SetActive(true);
    }
}
