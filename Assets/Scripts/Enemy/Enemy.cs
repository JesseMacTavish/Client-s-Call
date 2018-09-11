using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("The amount of health the enemy has")]
    [SerializeField] private int _health = 20;

    private EnemyStates _state;

    // Use this for initialization
    void Start()
    {
        EnemyHandler.Instance.Enemies.Add(gameObject);

        _state = GetComponent<EnemyStates>();
    }

    public void Hit(int pDamage)
    {
        _health -= pDamage;
        if (_health <= 0)
        {
            die();
            return;
        }

        _state.ChangeState(EnemyStates.EnemyState.DAMAGED);
    }

    private void die()
    {
        EnemyHandler.Instance.Enemies.Remove(gameObject);
        Destroy(gameObject);
    }
}
