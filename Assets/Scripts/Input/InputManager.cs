using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    public static Vector2 Movement; //store the current movement input so other scripts can access it

    private PlayerInput _playerInput;
    private InputAction _moveAction;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
    }

    private void Update()
    {
        Movement = _moveAction.ReadValue<Vector2>(); //store it in the static Movement variable so other scripts can access it
    }

}
