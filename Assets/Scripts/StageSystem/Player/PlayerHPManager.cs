using UnityEngine;

namespace StageSystem.Player
{
public class PlayerHPManager : MonoBehaviour
{
    [SerializeField] float MaxHP = 5f;
    float _currentHP;
    
    public bool TakeDamage(float damage)
    {
        _currentHP -= damage;
        if (_currentHP <= 0){
            _currentHP = 0;
            Debug.Log("Player is dead!");
            return true; // プレイヤーが死んだことを示す
        }
        
        return false; // まだ生きていることを示す
    }
}
}
