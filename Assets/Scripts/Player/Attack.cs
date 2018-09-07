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
    public float AttackCooldown = 0.2f;

    private SpriteRenderer _renderer;
    private PlayerAnimation _animation;
    private EnemyHandler _handler;
    private Rigidbody _rigidbody;
    private BoxCollider _trigger;

    private float _cooldown = 0;

    private List<GameObject> _enemiesInRange;

    // Use this for initialization
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _animation = GetComponent<PlayerAnimation>();
        _handler = EnemyHandler.Instance;
        _rigidbody = GetComponent<Rigidbody>();
        _enemiesInRange = new List<GameObject>();

        _trigger = GetComponent<BoxCollider>();
        _trigger.size = new Vector3(Attackrange, _trigger.size.y, Attackrange);
    }

    // Update is called once per frame
    void Update()
    {
        if (_cooldown <= 0)
        {
            //if not doing a combo, press once. While doing a combo you can hold
            if (Input.GetButtonDown("Fire1") || (Input.GetButton("Fire1") && _animation.IsInCombo))
            {
                attack();
            }
        }
        else
        {
            _cooldown -= 1 * Time.deltaTime;
        }
    }

    public List<GameObject> Enemies
    {
        get
        {
            return _enemiesInRange;
        }
    }

    //TODO: make a decision, either seperate left and right or hit everyone around you

    /**
    private void attack(string pDirection)
    {
        int damage = DefaultDamage;

        _animation.AttackAnimation(); //play the animation
        if (_animation.IsInCombo) //if we're doing a combo: double damage
        {
            startCooldown(); //only start cooldown once you do a combo
            damage *= 2;
        }

        List<int> indices = new List<int>();

        pDirection = pDirection.ToLower();
        if (pDirection == "left")
        {
            foreach (GameObject enemy in _handler.Enemies) //loop through all enemies
            {
                if (enemy.GetComponent<Rigidbody>().position.x <= _rigidbody.position.x) //if enemy is to the left of us...
                {
                    Vector3 delta = transform.position - enemy.transform.position;
                    float distance = delta.magnitude;
                    if (distance <= Attackrange) //...and close enough
                    {
                        indices.Add(_handler.Enemies.IndexOf(enemy));
                        //enemy.GetComponent<Enemy>().Hit(damage); //then hit enemy
                    }
                }
            }
        }
        else
        {
            foreach (GameObject enemy in _handler.Enemies) //same as above, but then for right
            {
                if (enemy.GetComponent<Rigidbody>().position.x >= _rigidbody.position.x)
                {
                    Vector3 delta = transform.position - enemy.transform.position;
                    float distance = delta.magnitude;
                    if (distance <= Attackrange)
                    {
                        indices.Add(_handler.Enemies.IndexOf(enemy));
                        //enemy.GetComponent<Enemy>().Hit(damage);
                    }
                }
            }
        }

        foreach (int index in indices)
        {
            _handler.Enemies[index].GetComponent<Enemy>().Hit(damage);
        }
    }
    /**/

    private void attack()
    {
        int damage = DefaultDamage;

        _animation.AttackAnimation(); //play the animation
        if (_animation.IsInCombo) //if we're doing a combo: double damage
        {
            startCooldown(); //only start cooldown once you do a combo
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
            if (enemy.Hit())
            {
                _enemiesInRange.Remove(enemy.gameObject);
                i--;
            }
        }
    }

    private void startCooldown()
    {
        _cooldown = AttackCooldown;
    }

}
