using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Tooltip("The range in which you will hit enemies")]
    public float Attackrange = 1;

    [Tooltip("The damage of a standard single attack")]
    public int DefaultDamage = 10;

    [Tooltip("The force of the attack that throws an enemy up")]
    public float AttackForce = 15;

    private PlayerAnimation _animation;
    private BoxCollider _trigger;

    private bool _pressedAttack;
    private int _combo = 0;

    private List<GameObject> _enemiesInRange;

    void Start()
    {
        _animation = GetComponent<PlayerAnimation>();
        _enemiesInRange = new List<GameObject>();

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

            _pressedAttack = true;

            _animation.AttackAnimation();
        }
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

        if (!_pressedAttack)
        {
            _combo = 0;
            _animation.StopAll();
            return;
        }

        _pressedAttack = false;

        damage *= (_combo + 1);
        _combo++;

        for (int i = 0; i < _enemiesInRange.Count; i++)
        {
            if (_enemiesInRange[i] == null)
            {
                _enemiesInRange.RemoveAt(i);
            }

            if (GetComponent<SpriteRenderer>().flipX)
            {
                EnemyAttack enemy = _enemiesInRange[i].GetComponent<EnemyAttack>();
                if (enemy.GetComponent<Rigidbody>().position.x <= GetComponent<Rigidbody>().position.x)
                {
                    if (false)//TODO: enemy.Hit(damage))
                    {
                        _enemiesInRange.RemoveAt(i);
                        i--;
                    }
                }
            }
            else
            {
                EnemyAttack enemy = _enemiesInRange[i].GetComponent<EnemyAttack>();
                if (enemy.GetComponent<Rigidbody>().position.x >= GetComponent<Rigidbody>().position.x)
                {
                    if (false)//TODO: enemy.Hit(damage))
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
        for (int i = 0; i < _enemiesInRange.Count; i++)
        {
            if (_enemiesInRange[i] == null)
            {
                _enemiesInRange.RemoveAt(i);
            }

            if (GetComponent<SpriteRenderer>().flipX)
            {
                EnemyAttack enemy = _enemiesInRange[i].GetComponent<EnemyAttack>();
                if (enemy.GetComponent<Rigidbody>().position.x <= GetComponent<Rigidbody>().position.x)
                {
                    //TODO: enemy.Flyup(AttackForce);
                }
            }
            else
            {
                EnemyAttack enemy = _enemiesInRange[i].GetComponent<EnemyAttack>();
                if (enemy.GetComponent<Rigidbody>().position.x >= GetComponent<Rigidbody>().position.x)
                {
                    //TODO: enemy.Flyup(AttackForce);
                }
            }
        }
    }
}
