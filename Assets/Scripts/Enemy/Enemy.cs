﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject dust;

    [Tooltip("The amount of health the enemy has")]
    [SerializeField] private int _health = 20;

    public Vector2 knockBackspeed;
    float knockSpeedX;
    float knockSpeedY;
    public float flyupSpeed = 0.2f;
    float _flightSpeed;
    private EnemyStates _state;
    bool _fly;
    bool _knockBack;

<<<<<<< HEAD
    private float _startY;
=======
>>>>>>> 441073a68afcd0ca00a26516ba8e88f169321b87
    Vector3 _oldPosition;

    void Start()
    {
        EnemyHandler.Instance.EnemySpawned(gameObject);

        _state = GetComponent<EnemyStates>();
        _startY = transform.position.y;
    }

    void Update()
    {
        if (_fly)
        {
            _flightSpeed -= 0.01f;

<<<<<<< HEAD
            if (_state.CurrentState == EnemyStates.EnemyState.FLYUP)
=======
            if (_state.StartState != EnemyStates.EnemyState.DAMAGED)
>>>>>>> 441073a68afcd0ca00a26516ba8e88f169321b87
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + _flightSpeed, transform.position.z);

                CancelInvoke();
            }

            if (transform.position.y <= _oldPosition.y)
            {
                transform.position = new Vector3(transform.position.x, _startY, transform.position.z);
                _fly = false;
                Invoke("changeStateRandom", 0.5f);

                //Spawn the dust under the guy
                float enemyHeight = GetComponent<SpriteRenderer>().sprite.texture.height;
                float dustHeight = dust.GetComponent<SpriteRenderer>().sprite.texture.height;
                float y = transform.position.y - (enemyHeight - dustHeight) / 200f;
                Instantiate(dust, new Vector3(transform.position.x, y, transform.position.z - 0.1f), Quaternion.identity);
            }

<<<<<<< HEAD
            if (_state.CurrentState == EnemyStates.EnemyState.DAMAGED || _state.CurrentState == EnemyStates.EnemyState.AIRDAMAGED)
            {
                _flightSpeed = 0.05f;
                _flightSpeed += 0.01f;
=======
            if (_state.CurrentState == EnemyStates.EnemyState.DAMAGED)
            {
                _flightSpeed = 0.05f;
                _flightSpeed += 0.03f;
>>>>>>> 441073a68afcd0ca00a26516ba8e88f169321b87
            }
        }
        if (_knockBack)
        {
<<<<<<< HEAD
            knockSpeedX -= 0.01f;
            if (knockBackspeed.x < 0)
                knockBackspeed.x = 0;
            knockSpeedY -= 0.01f;
=======
>>>>>>> 441073a68afcd0ca00a26516ba8e88f169321b87

            if (_state.CurrentState != EnemyStates.EnemyState.DAMAGED && _state.CurrentState != EnemyStates.EnemyState.AIRDAMAGED)
            {
                if (GetComponent<SpriteRenderer>().flipX)
<<<<<<< HEAD
=======
                    transform.position = new Vector3(transform.position.x + knockSpeedX, transform.position.y + knockSpeedY, transform.position.z);
                else
>>>>>>> 441073a68afcd0ca00a26516ba8e88f169321b87
                    transform.position = new Vector3(transform.position.x - knockSpeedX, transform.position.y + knockSpeedY, transform.position.z);
                else
                    transform.position = new Vector3(transform.position.x + knockSpeedX, transform.position.y + knockSpeedY, transform.position.z);
                CancelInvoke();
            }

            if (transform.position.y <= _oldPosition.y)
            {
<<<<<<< HEAD
                transform.position = new Vector3(transform.position.x, _startY, transform.position.z);
=======
                transform.position = new Vector3(transform.position.x, _oldPosition.y, transform.position.z);
            }
            else
            {
                knockSpeedY -= 0.01f;
            }
            knockSpeedX -= 0.01f;

            if (knockSpeedX <= 0)
            {
>>>>>>> 441073a68afcd0ca00a26516ba8e88f169321b87
                _knockBack = false;
                Invoke("changeStateRandom", 0.5f);
            }
        }
    }

    public bool Hit(int pDamage)
    {
        CancelInvoke();

        if (_state.CurrentState == EnemyStates.EnemyState.FLYUP || _state.CurrentState == EnemyStates.EnemyState.AIRDAMAGED)
        {
            _fly = true;
            _state.ChangeState(EnemyStates.EnemyState.AIRDAMAGED);
        }
        else
        {
            _state.ChangeState(EnemyStates.EnemyState.DAMAGED);
        }

        _health -= pDamage;

        if (_health <= 0)
        {
            die();
            return true;
        }

<<<<<<< HEAD

=======


        _state.ChangeState(EnemyStates.EnemyState.DAMAGED);
>>>>>>> 441073a68afcd0ca00a26516ba8e88f169321b87
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
        if (_state.CurrentState == EnemyStates.EnemyState.FLYUP)
        {
            return;
        }

        _state.ChangeState(EnemyStates.EnemyState.FLYUP);
        _fly = true;
        _oldPosition = transform.position;
        _flightSpeed = flyupSpeed;
    }

    public void KnockBack()
    {
        if (_state.CurrentState != EnemyStates.EnemyState.FLYUP && _state.CurrentState != EnemyStates.EnemyState.AIRDAMAGED)
        {
            return;
        }

        _state.ChangeState(EnemyStates.EnemyState.FLYUP);
        _knockBack = true;
        knockSpeedX = knockBackspeed.x;
        knockSpeedY = knockBackspeed.y;
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
        EnemyHandler.Instance.Attacked(gameObject);

        switch (random)
        {
            case 0:
                _state.ChangeState(EnemyStates.EnemyState.SURROUNDING);
                break;
            case 1:
                _state.ChangeState(EnemyStates.EnemyState.RETREAT);
                break;
            default:
                _state.ChangeState(EnemyStates.EnemyState.SURROUNDING);
                break;
        }
    }

    public void unFreezeAnimations()
    {
        GetComponent<Animator>().enabled = true;
    }
}
