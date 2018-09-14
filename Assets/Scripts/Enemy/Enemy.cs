﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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
            }

            if (_state.StartState == EnemyStates.EnemyState.DAMAGED)
            {
                _flightSpeed = 0.05f;
                _flightSpeed += 0.01f;
                //CancelInvoke();
            }
        }

        if (_knockBack)
        {
            knockSpeedX -= 0.01f;
            if (kncockBackspeed.x < 0)
                kncockBackspeed.x = 0;
            knockSpeedY -= 0.01f;

            if (_state.StartState != EnemyStates.EnemyState.DAMAGED)
            {
                if(GetComponent<SpriteRenderer>().flipX)
                    transform.position = new Vector3(transform.position.x + knockSpeedX, transform.position.y + knockSpeedY, transform.position.z);
                else
                    transform.position = new Vector3(transform.position.x - knockSpeedX, transform.position.y + knockSpeedY, transform.position.z);
                CancelInvoke();
            }

            if (transform.position.y <= _oldPosition.y)
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
        Debug.Log("Ouch");
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
