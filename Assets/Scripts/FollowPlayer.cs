using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject ObjectToFollow;
    public float _speed = 0.5f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (ObjectToFollow.transform.position.z < transform.position.z)
        {
            transform.position += new Vector3(0, 0, -_speed);
        }

        if (ObjectToFollow.transform.position.z > transform.position.z)
        {
            transform.position += new Vector3(0, 0, _speed);
        }

        if (ObjectToFollow.transform.position.x < transform.position.x)
        {
            transform.position += new Vector3(-_speed, 0, 0);
        }

        if (ObjectToFollow.transform.position.x > transform.position.z)
        {
            transform.position += new Vector3(_speed, 0, 0);
        }
    }
}
