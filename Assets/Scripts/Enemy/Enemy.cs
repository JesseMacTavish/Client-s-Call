using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("The amount of health the enemy has")]
    [SerializeField] private int _health = 20;

    public float horizontalFlight;
    private EnemyStates _state;
    private Animator _animator;
    bool _fly;

    Vector3 _oldPosition;
    Vector3 _peak;
    Vector3 _newPosition;
    Vector3 _flyDirection;

    void Start()
    {
        EnemyHandler.Instance.EnemySpawned(gameObject);

        _state = GetComponent<EnemyStates>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_fly)
        {
            if (_state.StartState != EnemyStates.EnemyState.DAMAGED)
            {
                transform.position += _flyDirection * 0.05f; //Hardcode
                CancelInvoke();
            }

            if (transform.position.y >= _peak.y && _flyDirection.y > 0)
            {
                _flyDirection.y *= -1f;
                _flyDirection = _flyDirection / 2f; //also hardcode
            }

            if (transform.position.y <= _oldPosition.y)
            {
                _fly = false;
                Invoke("changeStateRandom", 0.5f);
            }

            if (_state.StartState == EnemyStates.EnemyState.DAMAGED)
            {
                transform.position += _flyDirection.normalized * -0.01f; //Hardcode
                //CancelInvoke();
            }
        }

    }

    public bool Hit(int pDamage)
    {
        CancelInvoke();

        _health -= pDamage;
        _animator.Play("EnemyDamage");
        if (_health <= 0)
        {
            die();
            return true;
        }

        bool wasFly = false;
        if (_state.CurrentState == EnemyStates.EnemyState.FLYUP)
        {
            wasFly = true;
        }

        _state.ChangeState(EnemyStates.EnemyState.DAMAGED);
        if (_fly || wasFly)
        {
            Invoke("changeStateFly", 0.1f);
            transform.Translate(0, 0.3f, 0);
        }
        else
        {
            Invoke("changeStateRandom", 0.5f);
        }

        return false;
    }

    public void Fly(float pForce)
    {
        _state.ChangeState(EnemyStates.EnemyState.FLYUP);
        _fly = true;
        _oldPosition = transform.position;

        if (GetComponent<SpriteRenderer>().flipX)
        {
            _peak = new Vector3(transform.position.x + horizontalFlight, _oldPosition.y + pForce, transform.position.z);
        }
        else //Could be better
        {
            _peak = new Vector3(transform.position.x - horizontalFlight, _oldPosition.y + pForce, transform.position.z);
        }

        _flyDirection = _peak - _oldPosition;
    }

    private void die()
    {
        EnemyHandler.Instance.EnemyDied(gameObject);
    }

    private void changeState()
    {
        _state.ChangeState(EnemyStates.EnemyState.MOVING);
    }

    private void changeStateFly()
    {
        _state.ChangeState(EnemyStates.EnemyState.FLYUP);
    }

    private void changeStateRandom()
    {
        int random = Random.Range(0, 2);

        switch (random)
        {
            case 0:
                _state.ChangeState(EnemyStates.EnemyState.MOVING);
                break;
            case 1:
                _state.ChangeState(EnemyStates.EnemyState.RETREAT);
                break;
            default:
                _state.ChangeState(EnemyStates.EnemyState.MOVING);
                break;
        }
    }
}
