using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void AttackAnimation()
    {
        AnimatorStateInfo currentState = _animator.GetCurrentAnimatorStateInfo(0);
        
        if (currentState.IsName("PlayerAttack") || currentState.IsName("PlayerCombo"))
        {
            _animator.Play("PlayerCombo");
            return;
        }

        _animator.Play("PlayerAttack");
    }

    public bool IsAttacking
    {
        get
        {
            AnimatorStateInfo currentState = _animator.GetCurrentAnimatorStateInfo(0);
            return currentState.IsName("PlayerAttack") || currentState.IsName("PlayerCombo");
        }
    }

    public bool IsInCombo
    {
        get
        {
            AnimatorStateInfo currentState = _animator.GetCurrentAnimatorStateInfo(0);
            return currentState.IsName("PlayerCombo");
        }
    }


}
