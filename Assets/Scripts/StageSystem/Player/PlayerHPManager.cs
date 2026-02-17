using MainSystem.Audio;
using UnityEngine;

namespace StageSystem.Player
{
public class PlayerHPManager : MonoBehaviour
{
    [SerializeField] float MaxHP = 5f;
    float _currentHP;
    
    /// <summary>
    /// 敵からダメージを与える用の関数
    /// </summary>
    /// <param name="damage">ダメージ数</param>
    /// <returns>その結果死んだかどうか</returns>
    public bool TakeDamage(float damage)
    {
        _currentHP -= damage;
        AudioManager.Instance.PlaySE("PlayerDamageSE");
        
        if (_currentHP <= 0){
            _currentHP = 0;
            return true; // プレイヤーが死んだことを示す
            Death();
        }
        
        return false; // まだ生きていることを示す
    }

    /// <summary>
    /// 死んだ際に実行される関数
    /// </summary>
    void Death()
    {
        
    }
}
}
