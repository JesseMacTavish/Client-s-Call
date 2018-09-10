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

    private bool _playerWithinReach = false;
    private bool _playerAboveMe;

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
        if (ObjectToFollow == null || _state.CurrentState == EnemyStates.EnemyState.FLYUP)
        {
            return;
        }

        Rigidbody target = ObjectToFollow.GetComponent<Rigidbody>();

        lookAtTarget(target);

        if (!EnemyHandler.Instance.Attackers.Contains(gameObject))
        {
            changeStateIfWithinReach(target, EnemyStates.EnemyState.ATTACKING, EnemyStates.EnemyState.IDLE);

            return;
        }


        if (_state.CurrentState == EnemyStates.EnemyState.MOVING || _state.CurrentState == EnemyStates.EnemyState.ATTACKING)
        {
            _agent.destination = target.position;

            changeStateIfWithinReach(target, EnemyStates.EnemyState.ATTACKING, EnemyStates.EnemyState.MOVING);
        }
        else if (_state.CurrentState == EnemyStates.EnemyState.IDLE)
        {
            _state.CurrentState = EnemyStates.EnemyState.MOVING;
        }
    }

    private void lookAtTarget(Rigidbody pTarget)
    {
        if (pTarget.position.x < _rigidbody.position.x)
        {
            if (!_renderer.flipX)
            {
                _renderer.flipX = true;
                _agent.velocity = Vector3.zero;
            }
        }
        else
        {
            if (_renderer.flipX)
            {
                _renderer.flipX = false;
                _agent.velocity = Vector3.zero;
            }
        }

        if (pTarget.position.z < _rigidbody.position.z)
        {
            if (_playerAboveMe)
            {
                _playerAboveMe = false;
                _agent.velocity = Vector3.zero;
            }
        }
        else
        {
            if (!_playerAboveMe)
            {
                _playerAboveMe = true;
                _agent.velocity = Vector3.zero;
            }
        }
    }


    /// <summary>
    /// Change the state if less than [NavMeshAgent.stoppingDistance] away from pTarget
    /// </summary>
    /// <param name="pTarget">
    /// The Rigidbody of the target
    /// </param>
    /// <param name="pTrue">
    /// The state to change to if within reach
    /// </param>
    /// <param name="pFalse">
    /// The state to change to if not within reach
    /// </param>
    private void changeStateIfWithinReach(Rigidbody pTarget, EnemyStates.EnemyState pTrue, EnemyStates.EnemyState pFalse)
    {
        if (_playerWithinReach)
        {
            _state.CurrentState = pTrue;
        }
        else
        {
            _state.CurrentState = pFalse;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerWithinReach = true;
            List<GameObject> enemies = other.GetComponent<Attack>().Enemies;

            if (!enemies.Contains(gameObject))
            {
                enemies.Add(gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float distance = (other.GetComponent<Rigidbody>().position - _rigidbody.position).magnitude;
            if (distance < other.GetComponent<BoxCollider>().size.x)
            {
                return;
            }
            _playerWithinReach = false;
            other.GetComponent<Attack>().Enemies.Remove(gameObject);
        }
    }

    public bool InReach
    {
        get
        {
            return _playerWithinReach;
        }
    }
}
