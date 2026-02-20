using System;
using InputSystemActions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StageSystem.Player
{
public class YMover : MonoBehaviour
{
    void Start()
    {
        _inputActions = new InputActions();
        _inputActions.Player.Move.performed += OnMove;
        _inputActions.Player.Move.canceled += MoveCancel;
        
        _inputActions.Enable();
    }

    void OnDestroy()
    {
        _inputActions.Player.Move.performed -= OnMove;
        _inputActions.Player.Move.canceled -= MoveCancel;
         _inputActions.Disable();
    }

    void Update()
    {
        transform.localPosition += new Vector3(0, _moveValue, 0) * (Time.deltaTime * 4);
    }
    InputActions _inputActions;
    float _moveValue;
    void OnMove(InputAction.CallbackContext context)
    {
        Debug.LogError("Move Input: " + context.ReadValue<Vector2>());
        _moveValue = context.ReadValue<Vector2>().y;
    }
    void MoveCancel(InputAction.CallbackContext context)
    {
        _moveValue = 0;
    }
}
}
