using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStates : MonoBehaviour
{
    public enum EnemyState
    {
        IDLE,
        MOVING,
        ATTACKING,
    }

    public EnemyState CurrentState;

    // Use this for initialization
    void Start()
    {
        CurrentState = EnemyState.IDLE;
    }
}
