using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gavel : MonoBehaviour
{
    public void StopAnimation()
    {
        GetComponent<Animator>().SetBool("GavelOnce", false);
        GetComponent<Animator>().SetBool("GavelThrice", false);
    }

    public void GavelHit()
    {
        AudioManager.instance.PlaySFX("gavel", 1f);
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
