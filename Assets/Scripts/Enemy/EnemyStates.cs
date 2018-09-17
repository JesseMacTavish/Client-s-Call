using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStates : MonoBehaviour
{
    [Tooltip("The state of the enemy at the beginning")]
    [SerializeField] private EnemyState _startState = EnemyState.SURROUNDING;

    public enum EnemyState
    {
        SURROUNDING,
        MOVING,
        ATTACKING,
        RETREAT,
        DAMAGED,
        FLYUP,
        AIRDAMAGED,
    }

    void Awake()
    {
        CurrentState = _startState;
    }

    private void Update()
    {
        //todo: be sure to get rid of this update method later
        _startState = CurrentState;
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
<<<<<<< HEAD
                GetComponent<EnemyDamaged>().DamageAnimation();
                //TODO: move this to EnemyDamaged.cs
                //if (GetComponent<SpriteRenderer>().flipX)
                //{
                //    transform.position += Vector3.left * -0.1f; //Hardcode
                //}
                //else
                //{
                //    transform.position += Vector3.right * -0.1f; //Hardcode
                //}
=======
>>>>>>> 441073a68afcd0ca00a26516ba8e88f169321b87
                break;
            case EnemyState.AIRDAMAGED:
                GetComponent<EnemyDamaged>().DamageAirAnimation();
                break;
            default:
                break;
        }

        GetComponent<EnemyMovement>().AddAvailableDegree();
    }

    public EnemyState CurrentState { get; private set; }
}
