using System.Collections;
using DG.Tweening;
using MainSystem.Audio;
using StageSystem.Player;
using UnityEngine;

namespace StageSystem.Enemy
{
public class NotEnemyEnemy : MonoBehaviour,IEnemy
{
    [SerializeField] float damage = 1f;
    
    /// <summary>
    /// ダメージを受けた時の処理
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void TakeDamage(float damage)
    {
        
    }

 
    void OnTriggerEnter(Collider collision)
    {
        var player = collision.GetComponentInParent<PlayerHealth>();
        var playerHPManager = collision.GetComponentInParent<PlayerHPManager>();

        if (player != null)
        {
            playerHPManager.TakeDamage(damage);
        }
    }
    
}
}
