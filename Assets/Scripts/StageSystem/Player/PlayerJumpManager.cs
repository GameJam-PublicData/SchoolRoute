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
    CancellationTokenSource _jumpCTS;
    bool _isJumping = false;
    int _groundCount;
    
    [Inject]
    public void Construct(IReadOnlyGravitySystem gravitySystem)
    {
        _gravitySystem = gravitySystem;
    }

    [SerializeField] float jumpForce = 15f; // ジャンプの力
    [SerializeField] float jumpMaxTime = 1f;
    
    float _currentJumpForce;
    bool _isJumpButtonPressed;
    
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
        if (_isJumping == true) return;
        if(_groundCount <= 0) return;
        _jumpCTS?.Cancel();
        _jumpCTS = new CancellationTokenSource();
        _isJumping = true;
        Debug.Log("Jump performed!");
        _isJumpButtonPressed = true;
        UniTask.Delay(TimeSpan.FromSeconds(jumpMaxTime), cancellationToken: _jumpCTS.Token).ContinueWith( () => _isJumpButtonPressed = false);
        JumpAsync(_jumpCTS.Token).Forget();

         
    }
    void OnJumpCanceled(InputAction.CallbackContext context)
    {
        _isJumpButtonPressed = false;
        if(_currentJumpForce > 1)
        {
            //_currentJumpForce = 0;
            //_currentJumpForce = (int)(_currentJumpForce * 0.7f);
        }
        if (_isJumping == false) return;
    }
    
    async UniTask JumpAsync(CancellationToken token)
    {
        _currentJumpForce = jumpForce;
        int timeElapsed = 0;
        while (!token.IsCancellationRequested && _isJumping)
        {
            if (_isJumpButtonPressed == false)
            {
                _currentJumpForce -= jumpForce/3f;
            }
            else
            {
                float newJumpForce = jumpForce * (1 - timeElapsed / (jumpMaxTime * 1000));

                _currentJumpForce = newJumpForce;
                
            }
            await UniTask.Delay(100, cancellationToken: token);
            timeElapsed += 100;
        }
    }

    void Update()
    {
        transform.localPosition += Vector3.up * (_currentJumpForce * Time.deltaTime);
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            _groundCount++;
            _isJumping = false;
            _currentJumpForce = 0;
            Debug.Log("Jump performed!");
            transform.DOKill();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
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