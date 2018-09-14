using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    [Tooltip("The time IN SECONDS between 2 enemy updates")]
    [SerializeField] private float _updateInterval = 1;

    [Tooltip("The damage the enemy does")]
    [SerializeField] private int _damage = 10;

    [Tooltip("The range in which the enemy can hit you")]
    public float Attackrange = 1;

    private EnemyStates _state;
    private EnemyMovement _reach;
    private Player _player;

    private float _time;
    private bool _attacked;
    private bool _attacking;

    // Use this for initialization
    void Start()
    {
        _state = GetComponent<EnemyStates>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        BoxCollider trigger = GetComponent<BoxCollider>();
        trigger.size = new Vector3(Attackrange, trigger.size.y, Attackrange);
    }

    // Update is called once per frame
    void Update()
    {
        if (_state.CurrentState == EnemyStates.EnemyState.ATTACKING)
        {
            if (!_attacking)
            {
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
        }
    }

    private void intervalUpdate()
    {
        if (!_attacked)
        {
            startAttack();
            return;
        }

        _attacked = false;
        changeState();
    }

    private void attack()
    {
        _attacked = true;
        _attacking = false;

        if (InReach)
        {
            _player.Hit(_damage);
        }
    }

    private void startAttack()
    {
        GetComponent<Animator>().Play("EnemyAttack");
        _attacking = true;
    }

    private EnemyStates.EnemyState randomState()
    {
        int random = Random.Range(0, 2);

        switch (random)
        {
            case 0:
                return EnemyStates.EnemyState.MOVING;
            case 1:
                return EnemyStates.EnemyState.RETREAT;
            default:
                return EnemyStates.EnemyState.MOVING;
        }
    }

    private void changeState()
    {
        //EnemyStates.EnemyState state = randomState();

        //_state.ChangeState(state);

        EnemyHandler.Instance.Attacked(gameObject);
        _state.ChangeState(EnemyStates.EnemyState.RETREAT);
    }

    public bool InReach
    {
        get
        {
            return ((_player.GetComponent<Rigidbody>().position - transform.position).magnitude <= Attackrange);
        }
    }
}
