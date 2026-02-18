using System;
using System.Collections.Generic;
using UnityEngine;

namespace StageSystem.Player
{
[CreateAssetMenu(menuName = "ScriptableObject/StageRoute")]
public class StageRouteSO : ScriptableObject
{
    [SerializeField]List<StageRouteData> routeDataList = new List<StageRouteData>();
    public IReadOnlyList<StageRouteData> RouteDataList => routeDataList;
}

[Serializable]
public class StageRouteData
{
    [Header("全てワールド軸でForwardがZ軸の正方向になるように設定すること")]
    public Direction GravityDirection;
    public Direction ForwardDirection;
    [Header("プレイヤーをどっち側から見るか")]
    public Direction CameraDirection;
    [SerializeField]Vector3 targetPosition;
    public Vector3 TargetPosition => targetPosition;
    [SerializeField] Vector3 jumpTargetPosition = Vector3.zero;
    public Vector3 JumpTargetPosition => jumpTargetPosition;
    public float CameraRotation;
    public float CameraDistance;

    public float MoveTime = 3;

}
}