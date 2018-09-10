using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Tooltip("The amount of health the enemy has")]
    [SerializeField] private float _health = 5;

    [Tooltip("The amount of damage the enemy does")]
    [SerializeField] private int _attackPower = 1;

    [Tooltip("The time IN SECONDS it takes for the enemy to attack")]
    [SerializeField] private int _attackCooldown = 2;

    [Tooltip("The time IN SECONDS it takes for the enemy to reach the highest point when punched up")]
    [SerializeField] private float _knockUpTime = 1;

    [Tooltip("The time IN SECONDS the enemy will fly still in the air")]
    [SerializeField] private float _flyTime = 1;

    private float _cooldown = 2;

    private EnemyHandler _handler;
    private EnemyStates _state;
    private Animator _animator;

    public Rigidbody Player;
    private Rigidbody _rigidbody;
    private NavMeshAgent _agent;
    private float _startY;

    private Vector3 _hitPosition;
    private Vector3 _hitDirection;
    private Vector3 _newPosition;
    private float _value;
    private float _timeInAir;

    // Use this for initialization
    void Start()
    {
        _state = GetComponent<EnemyStates>();
        _handler = EnemyHandler.Instance;
        _handler.Enemies.Add(gameObject);

        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
        _startY = _rigidbody.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (_state.CurrentState == EnemyStates.EnemyState.ATTACKING)
        {
            startAttack();
        }

        if (_state.CurrentState == EnemyStates.EnemyState.FLYUP)
        {
            if (_rigidbody.position.y <= _startY)
            {
                enable();
            }
            else
            {
                punchedUp();
            }
        }
    }

    public bool Hit(int pDamage = 1)
    {
        _health -= pDamage;

        if (_health <= 0)
        {
            die();
            return true;
        }

        return false;
    }

    private void die()
    {
        _handler.Enemies.Remove(gameObject);
        _handler.UpdateAttackers(gameObject);
        Destroy(gameObject);
    }

    private void startAttack()
    {
        if (_cooldown > 0)
        {
            _cooldown -= 1 * Time.deltaTime;
        }

        if (_cooldown <= 0)
        {
            _animator.Play("EnemyAttack");
            _cooldown = _attackCooldown;
            Invoke("attack", 0.5f);
        }
    }

    private void attack()
    {
        //float distance = (Player.position - _rigidbody.position).magnitude;
        bool inReach = GetComponent<FollowPlayer>().InReach;

        //if (distance <= _agent.stoppingDistance + 10)
        if(inReach)
        {
            Player.GetComponent<Player>().Hit(_attackPower);
        }
    }

    private void punchedUp()
    {
        if (_value >= 1)
        {
            flyStill();
            return;
        }

        _rigidbody.position = _hitPosition + (_hitDirection * Mathf.Lerp(0, 1, _value));

        _value += (1 / _knockUpTime) * Time.deltaTime;
    }

    private void flyStill()
    {
        _timeInAir += (1 / _flyTime) * Time.deltaTime;
        if (_timeInAir >= 1)
        {
            _rigidbody.useGravity = true;
        }
    }

    public void Flyup(float pForce)
    {
        if (_state.CurrentState != EnemyStates.EnemyState.FLYUP)
        {
            _rigidbody.velocity = Vector3.zero;
        }

        _state.CurrentState = EnemyStates.EnemyState.FLYUP;

        _agent.enabled = false;
        _hitPosition = _rigidbody.position;
        _rigidbody.useGravity = false;
        _value = 0;
        _timeInAir = 0;

        if (GetComponent<SpriteRenderer>().flipX == true)
        {
            //_rigidbody.AddForce(new Vector3(0.2f, 1, 0) * pForce, ForceMode.VelocityChange);
            _newPosition = _hitPosition + new Vector3(2.5f, 10, 0);
        }
        else
        {
            //_rigidbody.AddForce(new Vector3(-0.2f, 1, 0) * pForce, ForceMode.VelocityChange);
            _newPosition = _hitPosition + new Vector3(-2.5f, 10, 0);
        }

        _hitDirection = _newPosition - _hitPosition;
    }

    private void enable()
    {
        if (!EnemyHandler.Instance.Attackers.Contains(gameObject))
        {
            _state.CurrentState = EnemyStates.EnemyState.IDLE;
        }
        else
        {
            _state.CurrentState = EnemyStates.EnemyState.MOVING;
        }
        
        _rigidbody.velocity = Vector3.zero;
        _agent.enabled = true;
    }
}
