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
    [SerializeField]Vector3 targetPosition;
    public Vector3 TargetPosition => targetPosition;
    public float MoveSpeed;

}
}