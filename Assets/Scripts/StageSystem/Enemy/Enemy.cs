using System;
using UnityEngine;

namespace StageSystem.Enemy
{
public class Enemy : MonoBehaviour, IEnemy
{
    public void TakeDamage(float damage)
    {
        Debug.Log("Enemy took " + damage + " damage!");
    }
}
}