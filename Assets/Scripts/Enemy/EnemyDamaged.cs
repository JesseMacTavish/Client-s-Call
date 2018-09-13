using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamaged : MonoBehaviour
{
    private EnemyStates _state;

    // Use this for initialization
    void Start()
    {
        _state = GetComponent<EnemyStates>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
