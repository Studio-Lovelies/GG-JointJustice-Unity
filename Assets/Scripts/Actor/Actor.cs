using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(Animator))]
public class Actor : MonoBehaviour
{
    private Animator _animator;
    private ActorData _actorData;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void SetUpActor(ActorData actorData)
    {
        _actorData = actorData;
        _animator.runtimeAnimatorController = actorData.AnimatorController;
    }
}
