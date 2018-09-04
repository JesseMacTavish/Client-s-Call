using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float Attackrange = 10;
    private SpriteRenderer _renderer;

    // Use this for initialization
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_renderer.flipX)
            {
                attack("left");
            }
            else
            {
                attack("right");
            }
        }
    }

    private void attack(string pDirection)
    {
        List<GameObject> enemies = new List<GameObject>(Enemy.Enemies);

        pDirection = pDirection.ToLower();
        if (pDirection == "left")
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy.transform.position.x <= transform.position.x)
                {
                    Vector3 delta = transform.position - enemy.transform.position;
                    float distance = delta.magnitude;
                    if (distance <= Attackrange)
                    {
                        enemy.GetComponent<Enemy>().Hit();
                    }
                }
            }
        }
        else
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy.transform.position.x >= transform.position.x)
                {
                    Vector3 delta = transform.position - enemy.transform.position;
                    float distance = delta.magnitude;
                    if (distance <= Attackrange)
                    {
                        enemy.GetComponent<Enemy>().Hit();
                    }
                }
            }
        }
    }
}
