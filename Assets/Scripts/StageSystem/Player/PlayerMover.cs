using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace StageSystem.Player
{
public class PlayerMover : MonoBehaviour
{
    [SerializeField] StageRouteSO stageRouteSO;
    IGravitySystem _gravitySystem;

    [Inject]
    public void Construct(IGravitySystem gravitySystem)
    {
        _gravitySystem = gravitySystem;
    }
    
    StageRouteData _currentRouteData;
    int _currentRouteIndex = 0;
    Vector3 _targetPosition;
    void Start()
    {
        JumpToNextRoute(stageRouteSO.RouteDataList[_currentRouteIndex]);
        UpdateRouteData();
    }
    void UpdateRouteData()
    {
        if (_currentRouteIndex < stageRouteSO.RouteDataList.Count)
        {
            _currentRouteData = stageRouteSO.RouteDataList[_currentRouteIndex];
            _gravitySystem.ChangeGravity(stageRouteSO.RouteDataList[_currentRouteIndex].GravityDirection);
            JumpToNextRoute(_currentRouteData);
            Debug.Log($"Route {_currentRouteIndex} started. Gravity: {_currentRouteData.GravityDirection}, Forward: {_currentRouteData.ForwardDirection}, Target: {_currentRouteData.TargetPosition}");
            _currentRouteIndex++;
            _targetPosition = GetTargetPosition(_currentRouteData);
            
        }
        else
        {
            // すべてのルートを完了した場合の処理（例: プレイヤーを停止させる）
            _currentRouteData = null;
        }
    }

    void Update()
    {
        if(_currentRouteData == null) return;
        
        _targetPosition = GetTargetPosition(_currentRouteData);
        // プレイヤーの移動処理
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition , Time.deltaTime * _currentRouteData.MoveSpeed);
        
        // プレイヤーが目的地に到達したかどうかをチェック
        if (Vector3.Distance(transform.position, _targetPosition) < 0.01f)
        {
            UpdateRouteData();
        }
    }

    
    //重力が変わる時に角度をフェード
    void JumpToNextRoute(StageRouteData newData, float duration = 1f)
    {
        Vector3 newRotation = _gravityRotationMap[newData.GravityDirection];
        newRotation += _forwardDirectionMap[newData.ForwardDirection];
        Debug.Log($"New Rotation: {newRotation}+, Gravity: {newData.GravityDirection}, Forward: {newData.ForwardDirection}");
        if(duration == 0f)
        {
            //transform.localRotation = Quaternion.Euler(newRotation);
            return;
        }
        
        transform.localRotation = Quaternion.Euler(newRotation);
        //transform.DORotate(newRotation, 1f).SetEase(Ease.InOutSine);
        // ここで、oldRotationからnewRotationへのフェード処理を実装
        // 例えば、Quaternion.Lerpを使用して回転を滑らかに変化させることができます。
    }
    
    Dictionary<Direction, Vector3> _gravityRotationMap = new Dictionary<Direction, Vector3>()
    {
        { Direction.Forward, new Vector3(0,0,90) },
        { Direction.Backward, new Vector3(0,0,-90) },
        { Direction.Left, new Vector3(0,0,-90) },
        { Direction.Right, new Vector3(0,0,90) },
        { Direction.Down,Vector3.zero },
        { Direction.Up, new Vector3(0,0,180) }
    };
    Dictionary<Direction,Vector3> _forwardDirectionMap = new Dictionary<Direction, Vector3>()
    {
        { Direction.Forward, new Vector3(0,0,0) },
        { Direction.Backward, new Vector3(0,180,0) },
        { Direction.Left, new Vector3(0,-90,0) },
        { Direction.Right, new Vector3(0,90,0) },
        { Direction.Down, new Vector3(90,0,0) },
        { Direction.Up,  new Vector3(-90,0,0) }
    };


    Vector3 GetTargetPosition(StageRouteData data)
    {
        switch (data.ForwardDirection)
        {
            case Direction.Forward:
                return new Vector3(transform.position.x, transform.position.y, data.TargetPosition.z);
            case Direction.Backward:
                return new Vector3(transform.position.x, transform.position.y, data.TargetPosition.z);
            case Direction.Left:
                return new Vector3(data.TargetPosition.x, transform.position.y, transform.position.z);
            case Direction.Right:
                return new Vector3(data.TargetPosition.x, transform.position.y, transform.position.z);
            case Direction.Down:
                return new Vector3(transform.position.x, data.TargetPosition.y, transform.position.z);
            case Direction.Up:
                return new Vector3(transform.position.x, data.TargetPosition.y, transform.position.z);
        }
        return transform.position; // デフォルトは現在の位置
    }
}


}

