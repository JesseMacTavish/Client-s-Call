using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("The amount of health the enemy has")]
    [SerializeField] private int _health = 20;

    private EnemyStates _state;
    private Animator _animator;

    // Use this for initialization
    void Start()
    {
        EnemyHandler.Instance.Enemies.Add(gameObject);

        _state = GetComponent<EnemyStates>();
        _animator = GetComponent<Animator>();
    }

    public bool Hit(int pDamage)
    {
        CancelInvoke();

        _health -= pDamage;
        _animator.Play("EnemyDamage");
        if (_health <= 0)
        {
            die();
            return true;
        }

        _state.ChangeState(EnemyStates.EnemyState.DAMAGED);
        Invoke("changeState", 0.5f);

        return false;
    }

    private void die()
    {
        EnemyHandler.Instance.Enemies.Remove(gameObject);
        Destroy(gameObject);
    }

    private void changeState()
    {
        _state.ChangeState(EnemyStates.EnemyState.MOVING);
    }
}
