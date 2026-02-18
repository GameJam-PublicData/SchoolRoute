using MainSystem.Audio;
using MainSystem.UI;
using UnityEngine;
using VContainer;

namespace StageSystem.Player
{
public class PlayerHPManager : MonoBehaviour
{
    IPlayerLifeUI _playerLifeUI;
    
    [SerializeField] float MaxHP = 5f;
    float _currentHP;
    
    [Inject]
    void Construct(IPlayerLifeUI playerLifeUI)
    {
        _playerLifeUI = playerLifeUI;
        _playerLifeUI.SetMaxHP(MaxHP);
    }
    
    /// <summary>
    /// 敵からダメージを与える用の関数
    /// </summary>
    /// <param name="damage">ダメージ数</param>
    /// <returns>その結果死んだかどうか</returns>
    public bool TakeDamage(float damage)
    {
        _currentHP -= damage;
        AudioManager.Instance.PlaySE("PlayerDamageSE");
        
        // UIの更新
        _playerLifeUI.UpdateLifeUI(_currentHP);
        
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
