﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Tooltip("The range in which you will hit enemies")]
    public float Attackrange = 20;

    [Tooltip("The damage of a standard single attack")]
    public int DefaultDamage = 10;

    [Tooltip("The force of the attack that throws an enemy up")]
    public float AttackForce = 15;

    [Tooltip("LeapLength, leapHeight\nActual leap is 2x longer than LeapLength")]
    public Vector2 LeapLengthAndHeight;

    private PlayerAnimation _animation;
    private BoxCollider _trigger;

    private bool _pressedAttack;
    private int _combo = 0;

    private List<GameObject> _enemiesInRange;

    private bool _leaping;
    private bool _highestPoint;
    private Vector3 _oldPosition;
    private Vector3 _newPosition;
    private Vector3 _leapDirection;
    private float _value;

<<<<<<< HEAD
=======
    private SpriteRenderer _renderer;

    // Use this for initialization
>>>>>>> ad75dbe608caa59f3c2afb36ffca7334a3540a67
    void Start()
    {
        _animation = GetComponent<PlayerAnimation>();
        _enemiesInRange = new List<GameObject>();
        _renderer = GetComponent<SpriteRenderer>();

        _trigger = GetComponent<BoxCollider>();
        _trigger.size = new Vector3(Attackrange, _trigger.size.y, Attackrange);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!_animation.IsAttacking)
            {
                _combo = 0;
            }
            else
            {
                _pressedAttack = true;
            }

            _animation.AttackAnimation();
        }

        if (_leaping)
        {
            leaping();
        }
    }

    private void continueAnimation()
    {
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            if (_renderer.flipX)
            {
                _renderer.flipX = false;
            }
        }
        else if (Input.GetAxisRaw("Horizontal") == -1)
        {
            if (!_renderer.flipX)
            {
                _renderer.flipX = true;
            }
        }

        if (!_pressedAttack)
        {
            _combo = 0;
            _animation.StopAll();
            return;
        }

        _combo++;
        _pressedAttack = false;
    }

    public List<GameObject> Enemies
    {
        get
        {
            return _enemiesInRange;
        }
    }

    private void attack()
    {
        int damage = DefaultDamage;

        damage *= (_combo + 1);

        for (int i = 0; i < _enemiesInRange.Count; i++)
        {
            if (_enemiesInRange[i] == null)
            {
                _enemiesInRange.RemoveAt(i);
            }

            if (GetComponent<SpriteRenderer>().flipX)
            {
                Enemy enemy = _enemiesInRange[i].GetComponent<Enemy>();
                if (enemy.GetComponent<Transform>().position.x <= GetComponent<Rigidbody>().position.x)
                {
                    if (enemy.Hit(damage))
                    {
                        _enemiesInRange.RemoveAt(i);
                        i--;
                    }
                }
            }
            else
            {
                Enemy enemy = _enemiesInRange[i].GetComponent<Enemy>();
                if (enemy.GetComponent<Transform>().position.x >= GetComponent<Rigidbody>().position.x)
                {
                    if (enemy.Hit(damage))
                    {
                        _enemiesInRange.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
    }

    private void throwEnemyUp()
    {
        int damage = DefaultDamage;

        damage *= (_combo + 1);

        for (int i = 0; i < _enemiesInRange.Count; i++)
        {
            if (_enemiesInRange[i] == null)
            {
                _enemiesInRange.RemoveAt(i);
            }

            if (GetComponent<SpriteRenderer>().flipX)
            {
                Enemy enemy = _enemiesInRange[i].GetComponent<Enemy>();
                if (enemy.GetComponent<Transform>().position.x <= GetComponent<Rigidbody>().position.x)
                {
                    if (enemy.Hit(damage))
                    {
                        _enemiesInRange.RemoveAt(i);
                        i--;
                        return;
                    }

                    enemy.Fly(AttackForce);
                }
            }
            else
            {
                Enemy enemy = _enemiesInRange[i].GetComponent<Enemy>();
                if (enemy.GetComponent<Transform>().position.x >= GetComponent<Rigidbody>().position.x)
                {
                    if (enemy.Hit(damage))
                    {
                        _enemiesInRange.RemoveAt(i);
                        i--;
                        return;
                    }

                    enemy.Fly(AttackForce);
                }
            }
        }
    }

    private void leap()
    {
        _oldPosition = transform.position;
        _newPosition = _oldPosition + (Vector3)LeapLengthAndHeight;
        _leapDirection = _newPosition - _oldPosition;
        if (GetComponent<SpriteRenderer>().flipX)
        {
            _leapDirection.x *= -1;
        }
        _newPosition = _oldPosition + _leapDirection;
        _value = 0;
        _highestPoint = false;
        _leaping = true;
    }

    private void leaping()
    {
        if (!_highestPoint)
        {
            _value += 1 / 3f;
        }
        else
        {
            _value += 1 / 2f;
        }

        transform.position = _oldPosition + _leapDirection * _value;

        if (_value >= 1)
        {
            _oldPosition = transform.position;
            _leapDirection.y *= -1;
            _value = 0;

            if (_highestPoint)
            {
                _leaping = false;
                return;
            }

            _highestPoint = true;
        }
    }
}
