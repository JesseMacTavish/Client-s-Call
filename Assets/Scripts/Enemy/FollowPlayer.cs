using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    [Tooltip("The time IN SECONDS between 2 enemy updates")]
    [SerializeField] private float _updateInterval = 1;

    [Tooltip("The speed at which the enemy will move towards the target")]
    [SerializeField] private float _speed = 0.1f;

    [Tooltip("The distance the enemy will stop from the target")]
    [SerializeField] private float _distanceFromTarget = 1;

    private Transform _transform;
    private SpriteRenderer _renderer;
    private EnemyStates _state;

    private GameObject _player;
    private Rigidbody _playerRigidbody;

    private bool _playerWithinReach;
    private float _time;

    private Vector3 _target;
    private Vector3 _offSet;

    public bool Skip;

    [HideInInspector] public Vector3 Direction;

    // Use this for initialization
    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _transform = GetComponent<Transform>();
        _state = GetComponent<EnemyStates>();

        _player = GameObject.FindGameObjectWithTag("Player");
        _playerRigidbody = _player.GetComponent<Rigidbody>();
        newOffset();
    }

    private void Update()
    {
        lookAtPlayer(_playerRigidbody.position);

        if (_state.CurrentState == EnemyStates.EnemyState.MOVING)
        {
            walkTowardsTarget();
        }

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

    private void intervalUpdate()
    {
        if (!Skip)
        {
            checkDistanceToEnemies();
        }

        Skip = false;
    }

    private void walkTowardsTarget()
    {
        Direction = getTarget() - _transform.position;
        Direction.y = 0;
        _transform.Translate(Direction.normalized * _speed);

        if (_playerWithinReach && Vector3.Distance(_transform.position, getTarget()) <= 4)
        {
            _state.ChangeState(EnemyStates.EnemyState.ATTACKING);
        }
    }

    private Vector3 getTarget()
    {
        _target = _playerRigidbody.position + _offSet;
        return _target;
    }

    private Vector3 newOffset()
    {
        float random = Random.Range(0f, 360f);
        _offSet = GetUnitVectorDegrees(random).normalized * _distanceFromTarget;
        return _offSet;
    }

    private Vector3 GetUnitVectorDegrees(float pDegrees)
    {
        float radians = Mathf.Deg2Rad * pDegrees;

        return new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians));
    }

    private void lookAtPlayer(Vector3 pPlayer)
    {
        if (pPlayer.x < _transform.position.x)
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
    }


    /// <summary>
    /// Change the state if within the trigger area of the pTarget
    /// </summary>
    /// <param name="pTarget">
    /// The Rigidbody of the target
    /// </param>
    /// <param name="pStateWithinReach">
    /// The state to change to if within reach
    /// </param>
    /// <param name="pStateNotWithinReach">
    /// The state to change to if not within reach
    /// </param>
    private void changeStateIfWithinReach(EnemyStates.EnemyState pStateWithinReach, EnemyStates.EnemyState pStateNotWithinReach)
    {
        if (_playerWithinReach)
        {
            _state.ChangeState(pStateWithinReach);
        }
        else
        {
            _state.ChangeState(pStateNotWithinReach);
        }
    }

    private void checkDistanceToEnemies()
    {
        foreach (GameObject enemy in EnemyHandler.Instance.Enemies)
        {
            if (enemy == gameObject)
            {
                continue;
            }

            if (Vector3.Distance(enemy.transform.position, _transform.position) < 1.5f)
            {
                //TODO: Do this properly

                //enemy.transform.Translate(Vector3.back * 2.5f);
                //_transform.Translate(Vector3.forward * 2.5f);

                //enemy.GetComponent<FollowPlayer>().Direction.rot;
                Vector3.RotateTowards(Direction, getTarget() + new Vector3(0, 0, 50), 6, _speed);

                //enemy.GetComponent<EnemyStates>().ChangeState(EnemyStates.EnemyState.MOVING);
                //_state.ChangeState(EnemyStates.EnemyState.MOVING);

                enemy.GetComponent<FollowPlayer>().Skip = true;
                Skip = true;
            }
        }
    }

    public void NewTarget()
    {
        newOffset();
    }

    public bool InReach
    {
        get
        {
            return _playerWithinReach;
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
            float distance = (other.GetComponent<Rigidbody>().position - _transform.position).magnitude;

            _playerWithinReach = false;
            other.GetComponent<Attack>().Enemies.Remove(gameObject);
        }
    }
}
