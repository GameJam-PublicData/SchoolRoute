using System;
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
        UpdateRouteData();
    }
    void UpdateRouteData()
    {
        if (_currentRouteIndex < stageRouteSO.RouteDataList.Count)
        {
            _currentRouteData = stageRouteSO.RouteDataList[_currentRouteIndex];
            _gravitySystem.ChangeGravity(stageRouteSO.RouteDataList[_currentRouteIndex].GravityDirection);
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




    // ルートデータから前進方向のみを取得するメソッド
    Vector3 GetActiveRouteDirectionVector(StageRouteData data)
    {
        return data.ForwardDirection switch
        {
            Direction.Up => Vector3.up,
            Direction.Down => Vector3.down,
            Direction.Left => Vector3.left,
            Direction.Right => Vector3.right,
            Direction.Forward => Vector3.forward,
            Direction.Backward => Vector3.back,
            _ => throw new ArgumentOutOfRangeException()
        };
    }


    Vector3 GetTargetPosition(StageRouteData data)
    {
        Vector3 directionVector = GetActiveRouteDirectionVector(data);
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

