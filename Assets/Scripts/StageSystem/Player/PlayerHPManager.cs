using System;
using Cysharp.Threading.Tasks;
using MainSystem.Audio;
using UnityEngine;

namespace StageSystem.Player
{
public class PlayerHPManager : MonoBehaviour
{
    [SerializeField] float MaxHP = 5f;
    [SerializeField]float _currentHP;
    [SerializeField] PlayerAnimationController playerAnimationController;

    void Start()
    {
        _currentHP = MaxHP;
    }

    /// <summary>
    /// 敵からダメージを与える用の関数
    /// </summary>
    /// <param name="damage">ダメージ数</param>
    /// <returns>その結果死んだかどうか</returns>
    public bool TakeDamage(float damage)
    {
        if(!_canDamaged)
        {  
            Debug .Log("Player is invincible and cannot take damage right now.");
            return false; // 無敵状態ならダメージを受けない
        }
        _currentHP -= damage;
        AudioManager.Instance.PlaySE("PlayerDamageSE");
        playerAnimationController.PlayerDamaged();
        
        DamageInterval().Forget(); // ダメージを受けてから次にダメージを受けるまでの無敵時間を開始
        if (_currentHP <= 0){
            _currentHP = 0;
            Death();
            return true; // プレイヤーが死んだことを示す
        }
        
        return false; // まだ生きていることを示す
    }
    
    [SerializeField] float interval = 1f;//ダメージを受けてから次にダメージを受けるまでの無敵時間
    bool _canDamaged = true;//ダメージを受けられるかどうか
    async UniTask DamageInterval( )
    {
        _canDamaged = false;
        await UniTask.Delay(System.TimeSpan.FromSeconds(interval));
        _canDamaged = true;
    }

    /// <summary>
    /// 死んだ際に実行される関数
    /// </summary>
    void Death()
    {
        Debug.LogError("Player has died! Implement death behavior here.");
    }
}
}
