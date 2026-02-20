using System;
using InputSystemActions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StageSystem.Player
{
public class YMover : MonoBehaviour
{
    public void SetInputEnabled(bool enabled) => isEnabled = enabled;
    [SerializeField] bool yInput = true;
    bool isEnabled = true;
    
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
        if (!isEnabled) return;
        Debug.LogError("Move Input: " + context.ReadValue<Vector2>());
        if(yInput) _moveValue = context.ReadValue<Vector2>().y;
        else _moveValue = context.ReadValue<Vector2>().x;
    }
    void MoveCancel(InputAction.CallbackContext context)
    {
        _moveValue = 0;
    }
}
}
