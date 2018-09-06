using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Health = 3;

    public void Hit(int pDamage)
    {
        Health -= pDamage;

        if (Health <= 0)
        {
            //Die
            //Destroy(gameObject);
        }
    }
}
