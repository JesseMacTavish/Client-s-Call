using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Transform _transform;
    private Vector3 _velocity;
    private Vector3 _accelleration;
    public float _speed = 0.75f;
    private Camera mainCamera;
    private Vector3 cameraPos;
    private SpriteRenderer _renderer;

    // Use this for initialization
    void Start()
    {
        _transform = GetComponent<Transform>();
        _accelleration = new Vector3(0, 0);
        _velocity = new Vector3(0, 0);
        mainCamera = Camera.main;
        cameraPos = mainCamera.transform.position;
        _renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            _accelleration += new Vector3(_speed, 0);

            if (_renderer.flipX)
            {
                _renderer.flipX = false;
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            _accelleration += new Vector3(-_speed, 0);

            if (!_renderer.flipX)
            {
                _renderer.flipX = true;
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            _accelleration += new Vector3(0, 0, _speed * 3);
        }

        if (Input.GetKey(KeyCode.S))
        {
            _accelleration += new Vector3(0, 0, -_speed * 3);
        }

        addVelocity();
    }

    private void addVelocity()
    {
        _velocity += _accelleration;

        if (_velocity.magnitude > _speed * 3)
        {
            _velocity.Normalize();
            _velocity *= _speed * 2.5f;
        }

        _transform.position += _velocity;

        cameraPos.x = _transform.position.x;
        mainCamera.transform.position = cameraPos;

        _accelleration.Set(0, 0, 0);
        _velocity.Set(0, 0, 0);
    }
}
