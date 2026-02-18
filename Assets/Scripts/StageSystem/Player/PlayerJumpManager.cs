using System;
using System.Collections.Generic;
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
    [SerializeField]bool isJumping = true;
    [SerializeField]int groundCount;

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
        JumpAsync(_jumpCTS.Token).Forget();
    }
    public void Jump(float force,Direction direction)
    {
        isJumping = true;
        Debug.Log("Jump performed!!!!!");
        _currentJumpForce = force;

        /*
        switch (direction)
        {
            case Direction.Up:
            case Direction.Down:
            {
                transform.DOLocalMoveY(0, 0.3f);
                transform.DOLocalMoveX(0, 0.3f);
                break;
            }
            case Direction.Backward:
            case Direction.Forward:
            {
                transform.DOLocalMoveY(0, 0.3f);
                transform.DOLocalMoveZ(0, 0.3f);
                break;
            }
            case Direction.Left:
            case Direction.Right:
            { 
                transform.DOLocalMoveY(0, 0.3f);
                transform.DOLocalMoveX(0, 0.3f);
                break;
            }
        }*/
        
    }
    
    void OnEnable()
    {
        _inputActions.Enable();
    }
    
    void OnJumpEnabled(InputAction.CallbackContext context)
    {
        Debug.Log("JumpInput");
        if (isJumping == true) return;
        if(groundCount <= 0) return;
        isJumping = true;
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
            if( isJumping == false) continue;
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
        transform.localRotation = Quaternion.Euler(_gravityRotationMap[_gravitySystem.GetGravityDirection()]);
        if(isJumping == false) return;
        
        Vector3 vec = _gravitySystem.OppositeDirections[_gravitySystem.GetGravityDirection()];
        vec *= -1;// 重力の反対方向に移動
        
        transform.localPosition += vec * (_currentJumpForce * Time.deltaTime);
    }

    Dictionary<Direction, Vector3> _gravityRotationMap = new Dictionary<Direction, Vector3>()
    {
        { Direction.Forward, new Vector3(-90, 0, 0) },
        { Direction.Backward, new Vector3(90, 0, 0) },
        { Direction.Left, new Vector3(0, 0, -90) },
        { Direction.Right, new Vector3(0, 0, 90) },
        { Direction.Down, Vector3.zero },
        { Direction.Up, new Vector3(0, 0, 180) }
    };

        
        

    Vector3 GetGravityVector()
    {
        switch (_gravitySystem.GetGravityDirection())
        {
            case Direction.Up:
                return Vector3.up * _currentJumpForce;
            case Direction.Down:
                return Vector3.down * _currentJumpForce;
            case Direction.Left:
                return Vector3.left * _currentJumpForce;
            case Direction.Right:
                return Vector3.right * _currentJumpForce;
            case Direction.Forward:
                return Vector3.forward * _currentJumpForce;
            case Direction.Backward:
                return Vector3.back * _currentJumpForce;
            default:
                return Vector3.zero;
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            groundCount++;
            isJumping = false;
            _currentJumpForce = 0;
            Debug.Log("Now Grounded!");
            transform.DOKill();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
      
            groundCount--;
            if(groundCount <= 0)
            {
                isJumping = true;
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