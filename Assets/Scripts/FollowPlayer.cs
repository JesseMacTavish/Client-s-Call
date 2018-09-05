using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject ObjectToFollow;
    public float _speed = 0.5f;

    Vector3 up;
    Vector3 down;
    Vector3 left;
    Vector3 right;

    // Use this for initialization
    void Start()
    {
        up = new Vector3(0, 0, _speed);
        down = new Vector3(0, 0, -_speed);
        left = new Vector3(-_speed, 0, 0);
        right = new Vector3(_speed, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (ObjectToFollow.transform.position.z < transform.position.z)
        {
            transform.position += down;
        }

        if (ObjectToFollow.transform.position.z > transform.position.z)
        {
            transform.position += up;
        }

        if (ObjectToFollow.transform.position.x < transform.position.x)
        {
            transform.position += left;
        }

        if (ObjectToFollow.transform.position.x > transform.position.z)
        {
            transform.position += right;
        }
    }
}
