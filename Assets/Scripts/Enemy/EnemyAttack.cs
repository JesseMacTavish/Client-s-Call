using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    [Tooltip("The time IN SECONDS between 2 enemy updates")]
    [SerializeField] private float _updateInterval = 1;

    private EnemyStates _state;

    private float _time;
    private bool _attacked;

    // Use this for initialization
    void Start()
    {
        _state = GetComponent<EnemyStates>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_state.CurrentState == EnemyStates.EnemyState.ATTACKING)
        {
            if (_time >= _updateInterval)
            {
                intervalUpdate();
                _time = 0;
            }
            else
            {
                _time += 1 * Time.deltaTime;
            }
        }
    }

    private void intervalUpdate()
    {
        if (!_attacked)
        {
            attack();
            return;
        }

        _attacked = false;
        changeState();
    }

    private void attack()
    {
        _attacked = true;
        GetComponent<Animator>().Play("EnemyAttack");
    }

    private EnemyStates.EnemyState randomState()
    {
        int random = Random.Range(0, 2);

        switch (random)
        {
            case 0:
                return EnemyStates.EnemyState.MOVING;
            case 1:
                return EnemyStates.EnemyState.RETREAT;
            default:
                return EnemyStates.EnemyState.MOVING;
        }
    }

    private void changeState()
    {
        EnemyStates.EnemyState state = randomState();

        _state.ChangeState(state);
    }
}
