using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject dust;

    [Tooltip("The amount of health the enemy has")]
    [SerializeField] private int _health = 20;

    public Vector2 kncockBackspeed;
    float knockSpeedX;
    float knockSpeedY;
    public float flyupSpeed = 0.2f;
    float _flightSpeed;
    private EnemyStates _state;
    private Animator _animator;
    bool _fly;
    bool _knockBack;

    Vector3 _oldPosition;

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
            _flightSpeed -= 0.01f;

            if (_state.StartState != EnemyStates.EnemyState.DAMAGED)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + _flightSpeed, transform.position.z);

                CancelInvoke();
            }

            if (transform.position.y <= _oldPosition.y)
            {
                _fly = false;
                Invoke("changeStateRandom", 0.5f);

                //Spawn the dust under the guy
                float enemyHeight = GetComponent<SpriteRenderer>().sprite.texture.height;
                float dustHeight = dust.GetComponent<SpriteRenderer>().sprite.texture.height;
                float y = transform.position.y - (enemyHeight - dustHeight) / 200f;
                Instantiate(dust, new Vector3(transform.position.x, y, transform.position.z - 0.1f), Quaternion.identity);
            }

            if (_state.CurrentState == EnemyStates.EnemyState.DAMAGED)
            {
                _flightSpeed = 0.05f;
                _flightSpeed += 0.03f;
            }
        }
        if (_knockBack)
        {

            if (_state.StartState != EnemyStates.EnemyState.DAMAGED)
            {
                if (GetComponent<SpriteRenderer>().flipX)
                    transform.position = new Vector3(transform.position.x + knockSpeedX, transform.position.y + knockSpeedY, transform.position.z);
                else
                    transform.position = new Vector3(transform.position.x - knockSpeedX, transform.position.y + knockSpeedY, transform.position.z);
                CancelInvoke();
            }

            if (transform.position.y <= _oldPosition.y)
            {
                transform.position = new Vector3(transform.position.x, _oldPosition.y, transform.position.z);
            }
            else
            {
                knockSpeedY -= 0.01f;
            }
            knockSpeedX -= 0.01f;

            if (knockSpeedX <= 0)
            {
                _knockBack = false;
                Invoke("changeStateRandom", 0.5f);
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



        _state.ChangeState(EnemyStates.EnemyState.DAMAGED);
        if (_fly)
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

    public void Fly()
    {
        _state.ChangeState(EnemyStates.EnemyState.FLYUP);
        _fly = true;
        _oldPosition = transform.position;
        _flightSpeed = flyupSpeed;
    }

    public void KnockBack()
    {
        _state.ChangeState(EnemyStates.EnemyState.FLYUP);
        _knockBack = true;
        knockSpeedX = kncockBackspeed.x;
        knockSpeedY = kncockBackspeed.y;
        if (!_fly)
        {
            _oldPosition = transform.position;
        }
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

    public void unFreezeAnimations()
    {
        GetComponent<Animator>().enabled = true;
    }
}
