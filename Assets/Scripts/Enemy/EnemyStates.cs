using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStates : MonoBehaviour
{
    [Tooltip("The state of the enemy at the beginning")]
    public EnemyState StartState = EnemyState.SURROUNDING;

    public enum EnemyState
    {
        SURROUNDING,
        MOVING,
        ATTACKING,
        RETREAT,
        DAMAGED,
        FLYUP,
    }

    void Awake()
    {
        CurrentState = StartState;
    }

    private void Update()
    {
        StartState = CurrentState;
    }

    public void ChangeState(EnemyState pState)
    {
        CurrentState = pState;

        switch (pState)
        {
            case EnemyState.MOVING:
            case EnemyState.SURROUNDING:
                GetComponent<EnemyMovement>().NewTarget();
                break;
            case EnemyState.ATTACKING:
                break;
            case EnemyState.RETREAT:
                GetComponent<EnemyRetreat>().NewDirection();
                break;
            case EnemyState.FLYUP:
                break;
            case EnemyState.DAMAGED:
                //TODO: move this to EnemyDamaged.cs
                if (GetComponent<SpriteRenderer>().flipX)
                {
                    transform.position += Vector3.left * -0.1f; //Hardcode
                }
                else
                {
                    transform.position += Vector3.right * -0.1f; //Hardcode
                }
                break;
            default:
                break;
        }

        GetComponent<EnemyMovement>().AddAvailableDegree();
    }

    public EnemyState CurrentState { get; private set; }
}
