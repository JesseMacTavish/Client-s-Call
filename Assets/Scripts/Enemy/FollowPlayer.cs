using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    [Tooltip("The object that the AI will follow")]
    public GameObject ObjectToFollow;

    private EnemyStates _state;
    private Rigidbody _rigidbody;
    private NavMeshAgent _agent;
    private SpriteRenderer _renderer;

    // Use this for initialization

    private void Start()
    {
        _state = GetComponent<EnemyStates>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;

        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Rigidbody target = ObjectToFollow.GetComponent<Rigidbody>();

        if (!EnemyHandler.Instance.Attackers.Contains(gameObject))
        {
            if ((target.position - _rigidbody.position).magnitude <= _agent.stoppingDistance)
            {
                _state.CurrentState = EnemyStates.EnemyState.ATTACKING;
            }
            else
            {
                _state.CurrentState = EnemyStates.EnemyState.IDLE;
            }

            return;
        }

        if (_state.CurrentState == EnemyStates.EnemyState.MOVING || _state.CurrentState == EnemyStates.EnemyState.ATTACKING)
        {

            _agent.destination = target.position;

            if (target.position.x < _rigidbody.position.x)
            {
                if (!_renderer.flipX)
                {
                    _renderer.flipX = true;
                }
            }
            else
            {
                if (_renderer.flipX)
                {
                    _renderer.flipX = false;
                }
            }

            if ((target.position - _rigidbody.position).magnitude <= _agent.stoppingDistance)
            {
                _state.CurrentState = EnemyStates.EnemyState.ATTACKING;
            }
            else
            {
                _state.CurrentState = EnemyStates.EnemyState.MOVING;
            }
        }
    }
}
