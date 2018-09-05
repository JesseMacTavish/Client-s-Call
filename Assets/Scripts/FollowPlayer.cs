using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    public GameObject ObjectToFollow;
    private Rigidbody _rigidbody;
    private NavMeshAgent _agent;
    private SpriteRenderer _renderer;

    // Use this for initialization

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.destination = ObjectToFollow.transform.position;
        _agent.updateRotation = false;

        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Rigidbody target = ObjectToFollow.GetComponent<Rigidbody>();

        _agent.destination = target.position;
        if (target.position.x < _rigidbody.position.x)
        {
            _renderer.flipX = true;
        }
        else
        {
            _renderer.flipX = false;
        }
    }
}
