using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
    CancellationTokenSource _jumpCTS = new();
    bool _isJumping = true;
    int _groundCount;

    [Inject]
    public void Construct(IReadOnlyGravitySystem gravitySystem)
    {
        _gravitySystem = gravitySystem;
    }

    [SerializeField] float jumpForce = 15f; // ジャンプの力
    
    float _currentJumpForce;
    bool _isJumpButtonPressed;
    
    void Awake()
    {
        _inputActions = new InputActions();
        _inputActions.Player.Jump.Enable();
        _inputActions.Player.Jump.started += OnJumpEnabled;
        _inputActions.Player.Jump.canceled += OnJumpCanceled;
        //JumpAsync(_jumpCTS.Token).Forget();
    }
    
    void OnEnable()
    {
        _inputActions.Enable();
    }
    
    void OnJumpEnabled(InputAction.CallbackContext context)
    {
        Debug.Log("JumpInput");
        if (_isJumping == true) return;
        if(_groundCount <= 0) return;
        _isJumping = true;
        Debug.Log("Jump performed!");
        _isJumpButtonPressed = true;
        _currentJumpForce = jumpForce;
    }
    
    void OnJumpCanceled(InputAction.CallbackContext context)
    {
        _isJumpButtonPressed = false;
        if(_currentJumpForce > 0)
        {
            _currentJumpForce = (int)(_currentJumpForce * 0.5f);
        }
    }
    
    
    async UniTask JumpAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested )
        {
            await UniTask.Delay(100, cancellationToken: token);
            if( _isJumping == false) continue;
            if(!_isJumpButtonPressed)
            {
                _currentJumpForce -= jumpForce/3.5f;
            }
            else
            {
                _currentJumpForce -= jumpForce/8;
            }
            if(_currentJumpForce < 0)
            {
                _isJumpButtonPressed = false;
            }
        }
    }

    void Update()
    {
        if(_isJumping == false) return;
        Vector3 vec = _gravitySystem.OppositeDirections[_gravitySystem.GetGravityDirection()];
        vec *= -1;// 重力の反対方向に移動
        transform.localPosition += vec * (_currentJumpForce * Time.deltaTime);
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            _groundCount++;
            _isJumping = false;
            _currentJumpForce = 0;
            Debug.Log("Now Grounded!");
            transform.DOKill();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Now Jumping!");
            _groundCount--;
            if(_groundCount <= 0)
            {
                _isJumping = true;
            }
        }
    }
    


    void OnDisable()
    {
        _inputActions.Player.Jump.started -= OnJumpEnabled;
        _inputActions.Player.Jump.canceled -= OnJumpCanceled;
        _inputActions.Player.Jump.Disable();
    }
} 
}