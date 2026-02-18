using MainSystem.Audio;
using StageSystem.Player;
using UnityEngine;

namespace StageSystem.Enemy
{
public class Enemy : MonoBehaviour,IEnemy
{
    [SerializeField] float damage = 1f;
    
    /// <summary>
    /// ダメージを受けた時の処理
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void TakeDamage(float damage)
    {
        AudioManager.Instance.PlaySE("EnemyDamageSE");
        
        Destroy(gameObject);
    }

 
    void OnTriggerEnter(Collider collision)
    {
        var player = collision.GetComponentInParent<PlayerHPManager>();

        if (player != null)
        { 
            player.TakeDamage(damage);
        }
    }
}
}
