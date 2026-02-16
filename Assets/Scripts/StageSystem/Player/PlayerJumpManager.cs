using System;
using InputSystemActions;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace StageSystem.Player
{
/// <summary>
/// プレイヤーのジャンプと縦方向の移動を管理するクラス
/// </summary>
public class PlayerJumpManager : MonoBehaviour
{
    InputActions _inputActions;
    IReadOnlyGravitySystem _gravitySystem;
    
    [Inject]
    public void Construct(IReadOnlyGravitySystem gravitySystem)
    {
        // 重力システムの依存性を注入
        // ここでは、重力システムを使用してジャンプの高さや挙動を調整することができます
    }

    void Awake()
    {
        _inputActions = new InputActions();
        _inputActions.Player.Jump.Enable();
        _inputActions.Player.Jump.started += OnJumpEnabled;
        _inputActions.Player.Jump.canceled += OnJumpCanceled;
        
    }
    
    void OnEnable()
    {
        _inputActions.Enable();
    }
    
    void OnJumpEnabled(InputAction.CallbackContext context)
    {
        // ジャンプの処理をここに実装
        Debug.Log("Jump performed!");
    }

    void OnJumpCanceled(InputAction.CallbackContext context)
    {
        // ジャンプのキャンセル処理をここに実装
        Debug.Log("Jump canceled!");
    }
    
    
    
    
    void OnDisable()
    {
        
        _inputActions.Player.Jump.started -= OnJumpEnabled;
        _inputActions.Player.Jump.canceled -= OnJumpCanceled;
        _inputActions.Player.Jump.Disable();
    }
} 
}