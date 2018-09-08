using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Tooltip("The range in which you will hit enemies")]
    public float Attackrange = 10;

    [Tooltip("The damage of a standard single attack")]
    public int DefaultDamage = 1;

    [Tooltip("The cooldown in SECONDS it takes until you can attack again")]
    public float AttackCooldown = 0.5f;

    [Tooltip("The force of the attack that throws up an enemy")]
    public float AttackForce = 3;

    private PlayerAnimation _animation;
    private BoxCollider _trigger;

    private float _cooldown = 0;

    private List<GameObject> _enemiesInRange;

    // Use this for initialization
    void Start()
    {
        _animation = GetComponent<PlayerAnimation>();
        _enemiesInRange = new List<GameObject>();

        _trigger = GetComponent<BoxCollider>();
        _trigger.size = new Vector3(Attackrange, _trigger.size.y, Attackrange);
    }

    // Update is called once per frame
    void Update()
    {
        if (_cooldown <= 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                attack();
            }
        }
        else
        {
            _cooldown -= 1f * Time.deltaTime;
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
        //startCooldown();

        _animation.AttackAnimation(); //play the animation
        if (_animation.IsInCombo || _animation.IsInCombo2) //if we're doing a combo: double damage
        {
            damage *= 2;
        }

        //go through _enemiesInRange and call hit() on everything
        for (int i = 0; i < _enemiesInRange.Count; i++)
        {
            if (_enemiesInRange[i] == null)
            {
                _enemiesInRange.Remove(_enemiesInRange[i]);
            }

            Enemy enemy = _enemiesInRange[i].GetComponent<Enemy>();
            Debug.Log("Enemy Hit");

            if (_animation.IsInCombo2)
            {
                throwEnemyUp(enemy);
            }

            if (enemy.Hit(damage))
            {
                _enemiesInRange.Remove(enemy.gameObject);
                i--;
            }
        }
    }

    private void throwEnemyUp(Enemy pEnemy)
    {
        pEnemy.Flyup(AttackForce);
    }

    private void startCooldown()
    {
        _cooldown = AttackCooldown;
    }

}
