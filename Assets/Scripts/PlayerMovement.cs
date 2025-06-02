using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    //Movement speed of player
    [SerializeField] private float _moveSpeed = 5f;

    private Vector2 _movement; //stores player input direction
    private Rigidbody2D _rb; //rigid body to apply physics
    private Animator _animator; //animator to control animations
    //constants for animator parameter names
    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _lastVertical = "LastVertical";
    private const string _lastHorizontal = "LastHorizontal";

    private void Awake() //called when script instance is being loaded
    {
        //cache component improves readability
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update() //called once per frame
    {
        _movement.Set(InputManager.Movement.x, InputManager.Movement.y); //get movement input from custom input manager
        _rb.linearVelocity = _movement * _moveSpeed; //apply movement using rigid body linear velocity

        //update animator parameters
        _animator.SetFloat(_horizontal, _movement.x);
        _animator.SetFloat(_vertical, _movement.y);

        if (_movement != Vector2.zero) //if player is moving, update last known direction for idle animation
        {
            _animator.SetFloat(_lastHorizontal, _movement.x);
            _animator.SetFloat(_lastVertical, _movement.y);
        }
    }
}
