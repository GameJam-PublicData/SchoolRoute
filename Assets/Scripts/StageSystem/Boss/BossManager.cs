using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace StageSystem.Boss
{
public class BossManager : MonoBehaviour,IBoss
{
    //仕組み、アタックパターンをもとに、行動を実行する
    
    
    //アタックパターン
    [SerializeField] BossAttackPattern _attackPattern;
    BossAttackBehavior _attackBehavior;

    Coroutine _nowAttackCoroutine;
    
    void Awake()
    {
        _attackBehavior = GetComponent<BossAttackBehavior>();
    }

    public int HP { get; set; } = 3;
    int _nowForm = 0;
    
    public void BossStart()
    {
        _nowAttackCoroutine = StartCoroutine(Attack());
    }

    int _attackIndex = 0;
    IEnumerator Attack()
    {
        while (true)
        {
            //待つ
            yield return new WaitForSeconds(_attackPattern.BossAttackPatternsByHP[_nowForm].AttackInterval[_attackIndex]);

            //攻撃
            switch (_attackPattern.BossAttackPatternsByHP[_nowForm].AttackRoutine[_attackIndex])
            {
                case BossAttackType.ChockShot:
                    _attackBehavior.ChockShot();
                    break;
                case BossAttackType.PenShot:
                    _attackBehavior.PenShot();
                    break;
                case BossAttackType.AttendanceRecordShot:
                    _attackBehavior.AttendanceRecordShot();
                    break;
            }

            //攻撃インデックスを増加
            _attackIndex++;
            //上限だった場合は戻す
            if (_attackIndex >= _attackPattern.BossAttackPatternsByHP[_nowForm].AttackRoutine.Count)
                _attackIndex = 0;
        }
    }

    public void TakeDamage(int damage)
    {
        //HPを減らし攻撃ルーチンを止める
        HP -= damage;
        StopCoroutine(_nowAttackCoroutine);
        
        //死亡判定
        if (HP <= 0)
        {
            Death();
            return;
        }
        
        //死なない場合は攻撃ルーチンを再開する
        _nowForm++;
        _nowAttackCoroutine = StartCoroutine(Attack());
    }

    void Death()
    {
        
    }
}
}