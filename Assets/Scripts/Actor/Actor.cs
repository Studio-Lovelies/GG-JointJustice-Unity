using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(Animator))]
public class Actor : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void SetUpActor(ActorData actorData)
    {
        _animator.runtimeAnimatorController = actorData.AnimatorController;
    }
}
