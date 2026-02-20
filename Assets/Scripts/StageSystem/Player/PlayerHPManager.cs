using Cysharp.Threading.Tasks;
using MainSystem.Audio;
using MainSystem.UI;
using StageSystem.Result;
using UnityEngine;
using VContainer;

namespace StageSystem.Player
{
public class PlayerHPManager : MonoBehaviour
{
    IPlayerLifeUI _playerLifeUI;
    
    [SerializeField] float MaxHP = 5f;
    [SerializeField]float _currentHP;
    [SerializeField] PlayerAnimationController playerAnimationController;

    void Start()
    {
        _currentHP = MaxHP;
        
        AudioManager.Instance.PlayBGM("StageBGM");
    }

    
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
        if(!_canDamaged)
        {  
            Debug .Log("Player is invincible and cannot take damage right now.");
            return false; // 無敵状態ならダメージを受けない
        }
        _currentHP -= damage;
        //AudioManager.Instance.PlaySE("PlayerDamageSE");
        playerAnimationController .PlayerDamaged();
        
        // UIの更新
        _playerLifeUI.UpdateLifeUI(_currentHP);
        
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
        ResultManager.Instance.Lose();
    }
}
}
