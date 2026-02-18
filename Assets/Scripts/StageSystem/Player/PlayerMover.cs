using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace StageSystem.Player
{
public class PlayerMover : MonoBehaviour
{
    [SerializeField] StageRouteSO stageRouteSO;
    [SerializeField] float jumpTime = 1f;
    [SerializeField] public bool canMove = true;
    IGravitySystem _gravitySystem;
    [SerializeField] CameraSystem cameraSystem;
    [SerializeField] float gravityJumpForce = 10f;
    
    [Inject]
    public void Construct(IGravitySystem gravitySystem)
    {
        _gravitySystem = gravitySystem;
    }

    PlayerForward _playerForward;

    void Awake()
    {
        _playerForward = GetComponentInChildren<PlayerForward>();
    }

    StageRouteData _currentRouteData;
    int _currentRouteIndex = 0;
    Vector3 _targetPosition;
    float _currentSpeed = 1;
    
    void Start()
    {
        UpdateRouteData(true);
    }
    // 現在のルートデータを更新するメソッド
    void UpdateRouteData(bool isStart = false)
    {
        if (_currentRouteIndex >= stageRouteSO.RouteDataList.Count) return;
        transform.DOKill(); 
        
        var oldData = _currentRouteData;
        _currentRouteData = stageRouteSO.RouteDataList[_currentRouteIndex];
        Debug.Log($"Route {_currentRouteIndex} started. Gravity: {_currentRouteData.GravityDirection}, Forward: {_currentRouteData.ForwardDirection}, Target: {_currentRouteData.TargetPosition}");
        _targetPosition = _currentRouteData.TargetPosition;
        
        if (_currentRouteIndex!=0&&_currentRouteData.GravityDirection == stageRouteSO.RouteDataList[_currentRouteIndex - 1].GravityDirection)
        {
            //同じ重力方向の場合はプレイヤーの向きをすばやく変える
            _playerForward.ChangeForwardDirection(_currentRouteData.ForwardDirection,0.1f);
        }
        else _playerForward.ChangeForwardDirection(_currentRouteData.ForwardDirection);
        //カメラ更新(仮)//todo
        //cameraSystem.SetRotation(_currentRouteData.CameraDirection, _currentRouteData.CameraRotation, _currentRouteData.CameraDistance);
        UniTask.Delay(TimeSpan.FromSeconds(jumpTime)).ContinueWith(() =>
        {
            if (isStart) return;
            cameraSystem.SetRotation(_currentRouteData.CameraTargetLocalPosition, _currentRouteData.CameraLocalPosition, _currentRouteData.LookZRotation);
        }).Forget();
        //スピード更新
        _currentSpeed = Vector3.Distance(transform.position, _targetPosition) / _currentRouteData.MoveTime;
        if (isStart)
        {
            _gravitySystem.ChangeGravity(_currentRouteData.GravityDirection);
            _currentRouteIndex++;
            _gravitySystem.ChangeGravity(_currentRouteData.GravityDirection);
            return;
        }
        
        
        //ジャンプ処理
        if (oldData.JumpTargetPosition != Vector3.zero &&
            oldData.GravityDirection != _currentRouteData.GravityDirection) JumpToPosition(oldData).Forget();
        else if (oldData.JumpTargetPosition != Vector3.zero) Jump(oldData).Forget();
        else if ( oldData.GravityDirection != _currentRouteData.GravityDirection)
        {
            Debug.LogWarning("GravityChangeJump");
            _gravitySystem.ChangeGravity(_currentRouteData.GravityDirection);
            GetComponentInChildren<PlayerJumpManager>().Jump(gravityJumpForce ,_currentRouteData.GravityDirection);
        }
        
        
        _currentRouteIndex++;
    }

    void Update()
    {
        if(_currentRouteData == null) return;
        if(!canMove) return;
        // プレイヤーの移動処理
        
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition , Time.deltaTime * _currentSpeed);
        
        // プレイヤーが目的地に到達したかどうかをチェック
        if (Vector3.Distance(transform.position, _targetPosition) < 0.01f)
        {
            UpdateRouteData();
        }
    }

    async UniTask Jump(StageRouteData oldData)
    {
        Debug .LogWarning($"SimpleJump to {oldData.JumpTargetPosition} from {transform.position}");
        canMove = false;
        PlayerJumpManager jumpManager = GetComponentInChildren<PlayerJumpManager>();
        jumpManager.enabled = false;
        jumpManager.transform.DOLocalJump(jumpManager.transform.localPosition, 1f, 5, jumpTime).SetEase(Ease.OutSine);
        transform.DOMove(oldData.JumpTargetPosition, jumpTime).SetEase(Ease.OutSine);
        await UniTask.Delay(TimeSpan.FromSeconds(jumpTime),cancellationToken:this.GetCancellationTokenOnDestroy());
        jumpManager.enabled = true;
        canMove = true;
    }

    async UniTask JumpToPosition(StageRouteData data)
    {
        canMove = false;
        Debug.LogWarning($"TargetJump to {data.JumpTargetPosition} from {transform.position}");
        GetComponentInChildren<PlayerJumpManager>().Jump(gravityJumpForce,data.GravityDirection);
        GetComponentInChildren<Collider>().enabled = false;

        switch (data.GravityDirection)
        {
            case Direction.Up:
            case Direction.Down:
            {
                transform.DOMoveY(data.JumpTargetPosition.y, jumpTime).SetEase(Ease.OutSine);
                break;
            }
            case Direction.Backward:
            case Direction.Forward:
            {
                transform.DOMoveY(data.JumpTargetPosition.y, jumpTime).SetEase(Ease.OutSine);
                break;
            }
            case Direction.Left:
            case Direction.Right:
            {
                transform.DOMoveY(data.JumpTargetPosition.y, jumpTime).SetEase(Ease.OutSine);
                break;
            }
        }
        
        await UniTask.Delay(TimeSpan.FromSeconds(0.1),cancellationToken:this.GetCancellationTokenOnDestroy());
        _gravitySystem.ChangeGravity(_currentRouteData.GravityDirection);
        GetComponentInChildren<Collider>().enabled = true;
        await UniTask.Delay(TimeSpan.FromSeconds(jumpTime/2),cancellationToken:this.GetCancellationTokenOnDestroy());
        canMove = true;
        Debug.Log($"Arrived at {data.JumpTargetPosition}");
    }
}
}

