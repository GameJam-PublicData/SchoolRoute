using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace StageSystem.Boss
{
[CreateAssetMenu(fileName = "BossAttack", menuName = "Scriptable Objects/BossAttack")]
public class BossAttackPattern : ScriptableObject
{
    
    public List<BossAttackData> BossAttackPatternsByHP;
}

[System.Serializable]
public enum BossAttackType
{
    /// <summary>
    ///  チョーク投げ
    /// </summary>
    ChockShot,

    /// <summary>
    /// ペン投げ
    /// </summary>
    PenShot,

    /// <summary>
    /// 出席簿投げ
    /// </summary>
    AttendanceRecordShot,
    
    /// <summary>
    /// 出席簿とチョーク投げ
    /// </summary>
    AttendanceRecordAndChockShot
}

[System.Serializable]
public struct BossAttackData
{
    public List<BossAttackType> AttackRoutine;
    public List<float> AttackInterval;
}
}