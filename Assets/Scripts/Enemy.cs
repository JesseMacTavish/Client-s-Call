using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _hitCooldown = 1;
    [SerializeField] private float _health = 5;
    private float _cooldown = 0;

    public static List<GameObject> Enemies = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        Enemies.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (_cooldown > 0)
        {
            _cooldown -= 1 * Time.deltaTime;
        }
    }

    public void Hit(int pDamage = 1)
    {
        if (_cooldown <= 0)
        {
            _health -= pDamage;
            startCooldown();

            if (_health <= 0)
            {
                Enemies.Remove(gameObject);
                Destroy(gameObject);
            }
        }
    }

    private void startCooldown()
    {
        _cooldown = _hitCooldown;
    }
}
