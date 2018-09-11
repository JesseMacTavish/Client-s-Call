using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStates : MonoBehaviour
{
    public EnemyState CURRENTSTATE;

    public enum EnemyState
    {
        IDLE,
        MOVING,
        ATTACKING,
        RETREAT,
        DAMAGED,
        FLYUP,
    }
   
    void Start()
    {
        CurrentState = EnemyState.MOVING;
    }

    private void Update()
    {
        CURRENTSTATE = CurrentState;
    }

    public void ChangeState(EnemyState pState)
    {
        switch (pState)
        {
            case EnemyState.IDLE:
                break;
            case EnemyState.MOVING:
                GetComponent<FollowPlayer>().NewTarget();
                break;
            case EnemyState.ATTACKING:
                break;
            case EnemyState.RETREAT:
                GetComponent<EnemyRetreat>().NewDirection();
                break;
            case EnemyState.FLYUP:
                break;
            default:
                break;
        }

        CurrentState = pState;
    }

    public EnemyState CurrentState { get; private set; }
}
