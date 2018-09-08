using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Tooltip("The time in which it takes until the enemy can be damaged again")]
    [SerializeField] private float _damageCooldown = 1;

    [Tooltip("The amount of health the enemy has")]
    [SerializeField] private float _health = 5;

    [Tooltip("The amount of damage the enemy does")]
    [SerializeField] private int _attackPower = 1;

    [Tooltip("The time IN SECONDS it takes for the enemy to attack")]
    [SerializeField] public int AttackCooldown = 2;

    private float _cooldown = 0;
    private float _attackCooldown = 2;

    private EnemyHandler _handler;
    private EnemyStates _state;
    private Animator _animator;

    public Rigidbody Player;
    private Rigidbody _rigidbody;
    private NavMeshAgent _agent;

    // Use this for initialization
    void Start()
    {
        _state = GetComponent<EnemyStates>();
        _handler = EnemyHandler.Instance;
        _handler.Enemies.Add(gameObject);

        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_state.CurrentState == EnemyStates.EnemyState.ATTACKING)
        {
            startAttack();
        }

        if (_cooldown > 0)
        {
            _cooldown -= 1 * Time.deltaTime;
        }
    }

    public bool Hit(int pDamage = 1)
    {
        if (_cooldown <= 0)
        {
            _health -= pDamage;
            startCooldown();

            if (_health <= 0)
            {
                die();
                return true;
            }
        }
        return false;
    }


    private void startCooldown()
    {
        _cooldown = _damageCooldown;
    }

    private void die()
    {
        _handler.Enemies.Remove(gameObject);
        _handler.UpdateAttackers(gameObject);
        Destroy(gameObject);
    }

    private void startAttack()
    {
        if (_attackCooldown > 0)
        {
            _attackCooldown -= 1 * Time.deltaTime;
        }

        if (_attackCooldown <= 0)
        {
            _animator.Play("EnemyAttack");
            _attackCooldown = AttackCooldown;
            Invoke("attack", 0.5f);
        }
    }

    private void attack()
    {
        float distance = (Player.position - _rigidbody.position).magnitude;

        if (distance <= _agent.stoppingDistance + 10)
        {
            Player.GetComponent<Player>().Hit(_attackPower);
        }
    }

    public void Flyup(float pForce)
    {
        _state.CurrentState = EnemyStates.EnemyState.FLYUP;
        _rigidbody.velocity = Vector3.zero;

        GetComponent<NavMeshAgent>().enabled = false;

        if (GetComponent<SpriteRenderer>().flipX == true)
        {
            _rigidbody.AddForce(new Vector3(0.2f, 1, 0) * pForce, ForceMode.VelocityChange);
        }
        else
        {
            _rigidbody.AddForce(new Vector3(-0.2f, 1, 0) * pForce, ForceMode.VelocityChange);
        }
    }
}
