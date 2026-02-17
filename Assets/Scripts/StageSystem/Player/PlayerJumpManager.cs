using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using InputSystemActions;
using StageSystem.Animation;
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
    bool _isJumping = true;
    int _groundCount;
    Vector3 _lastGroundedPos;
    
    [SerializeField] Fade fade;

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
        _jumpCTS?.Cancel();
        _jumpCTS = new CancellationTokenSource();
        _isJumping = true;
        Debug.Log("Jump performed!");
        _isJumpButtonPressed = true;
        JumpAsync(_jumpCTS.Token).Forget();

         
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
        _currentJumpForce = jumpForce;
        while (!token.IsCancellationRequested && _isJumping)
        {
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

            await UniTask.Delay(100, cancellationToken: token);
        }
    }

    void Update()
    {
        if (_isJumping == false)
        {
            // 地面にいる場合は最後の地面の位置を更新
            _lastGroundedPos = transform.localPosition;
        }
        else
        {
            Vector3 vec = _gravitySystem.OppositeDirections[_gravitySystem.GetGravityDirection()];
            vec *= -1;// 重力の反対方向に移動
            transform.localPosition += vec * (_currentJumpForce * Time.deltaTime);
        }
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
        if(other.gameObject.CompareTag("DeathZone"))
        {
            // 元の位置に戻る処理
            var hpManager = GetComponent<PlayerHPManager>();
            if (hpManager.TakeDamage(1))
            {
                SetLastGroundedPos().Forget();
            }
        }
    }
    
    async UniTask SetLastGroundedPos(float duration = 1f)
    {
        // 死んだときのフェードインとフェードアウトの処理
        fade.FadeIn(duration);
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        
        // プレイヤーを最後の地面の位置に戻す
        transform.localPosition = _lastGroundedPos;
        
        // フェードアウト
        fade.FadeOut(duration);
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