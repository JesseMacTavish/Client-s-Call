using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _health = 5;
    public static List<GameObject> Enemies = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        Enemies.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Hit()
    {
        _health--;
        if (_health <= 0)
        {
            Enemies.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
