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

    private float _cooldown = 0;

    // Use this for initialization
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _animation = GetComponent<PlayerAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_cooldown <= 0)
        {
            //if not doing a combo, press once. While doing a combo you can hold
            if (Input.GetButtonDown("Fire1") || (Input.GetButton("Fire1") && _animation.IsInCombo))
            {
                if (_renderer.flipX) //attack in the direction we're looking
                {
                    attack("left");
                }
                else
                {
                    attack("right");
                }
            }
        }
        else
        {
            _cooldown -= 1 * Time.deltaTime;
        }
    }

    private void attack(string pDirection)
    {
        int damage = DefaultDamage;

        _animation.AttackAnimation(); //play the animation
        if (_animation.IsInCombo) //if we're doing a combo: double damage
        {
            startCooldown(); //only start cooldown once you do a combo
            damage *= 2;
        }

        List<GameObject> enemies = new List<GameObject>(Enemy.Enemies);

        pDirection = pDirection.ToLower();
        if (pDirection == "left")
        {
            foreach (GameObject enemy in enemies) //loop through all enemies
            {
                if (enemy.transform.position.x <= transform.position.x) //if enemy is to the left of us...
                {
                    Vector3 delta = transform.position - enemy.transform.position;
                    float distance = delta.magnitude;
                    if (distance <= Attackrange) //...and close enough
                    {
                        enemy.GetComponent<Enemy>().Hit(damage); //then hit enemy
                    }
                }
            }
        }
        else
        {
            foreach (GameObject enemy in enemies) //same as above, but then for right
            {
                if (enemy.transform.position.x >= transform.position.x)
                {
                    Vector3 delta = transform.position - enemy.transform.position;
                    float distance = delta.magnitude;
                    if (distance <= Attackrange)
                    {
                        enemy.GetComponent<Enemy>().Hit(damage);
                    }
                }
            }
        }
    }

    private void startCooldown()
    {
        _cooldown = AttackCooldown;
    }
}
