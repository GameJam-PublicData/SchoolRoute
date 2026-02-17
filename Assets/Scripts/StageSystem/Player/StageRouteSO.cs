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
    public Direction GravityDirection;
    public Direction ForwardDirection;
    public Direction CameraDirection;
    [SerializeField]Vector3 targetPosition;
    public Vector3 TargetPosition => targetPosition;
    [SerializeField] Vector3 jumpTargetPosition = Vector3.zero;
    public Vector3 JumpTargetPosition => jumpTargetPosition;
    public float CameraDistance;
    
    public float MoveSpeed;

}
}