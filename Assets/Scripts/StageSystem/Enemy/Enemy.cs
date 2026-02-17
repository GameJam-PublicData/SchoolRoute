using System;
using MainSystem.Audio;
using StageSystem.Enemy;
using StageSystem.Player;
using UnityEngine;

public class Enemy : MonoBehaviour,IEnemy
{
    [SerializeField] float damage = 1f;
    [SerializeField] AudioManager audioManager;
    
    /// <summary>
    /// ダメージを受けた時の処理
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void TakeDamage(float damage)
    {
        audioManager.PlaySE("EnemyDamageSE");
        
        Destroy(gameObject);
    }

    /// <summary>
    /// プレイヤーに触れた時の処理
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        var player = collision.gameObject.GetComponent<PlayerHPManager>();

        if (player != null)
        { 
            player.TakeDamage(damage);
        }
    }
}
