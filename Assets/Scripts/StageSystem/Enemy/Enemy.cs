using System;
using StageSystem.Enemy;
using StageSystem.Player;
using UnityEngine;

public class Enemy : MonoBehaviour,IEnemy
{
    [SerializeField] float damage = 1f;
    
    public void TakeDamage(float damage)
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        var player = collision.gameObject.GetComponent<PlayerHPManager>();

        if (player != null)
        { 
            player.TakeDamage(damage);
        }
    }
}
