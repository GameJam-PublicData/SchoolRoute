using System;
using Cysharp.Threading.Tasks;
using InputSystemActions;
using StageSystem.Enemy;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StageSystem.Player
{
public class PlayerAttackManager : MonoBehaviour
{
    InputActions _input;
    [SerializeField] GameObject flickRangeObj;
    [SerializeField] float attackTime = 0.5f;
    
    bool _isAttacking;
    
    void Awake()
    {
        _input = new InputActions();
        _input.Player.Attack.Enable();
        
        _input.Player.Attack.started += OnFlickAsync;

        if (!flickRangeObj)
        {
            Debug.LogError("Flick Range Object is not assigned in the inspector.");
            return;
        }
        flickRangeObj.SetActive(false);
    }

    void OnDestroy()
    {
        _input.Player.Attack.started -= OnFlickAsync;
        
        _input.Player.Attack.Disable();
    }
    
    async void OnFlickAsync(InputAction.CallbackContext context)
    {
        if (_isAttacking) return;
        _isAttacking = true;
        
        try
        {
            // 攻撃範囲オブジェクトを有効化
            flickRangeObj.SetActive(true);
            
            // BoxColliderを使用して攻撃範囲内の敵を検出
            var sphere = flickRangeObj.GetComponent<BoxCollider>();
            var hits = Physics.OverlapBox(sphere.transform.position + sphere.center, sphere.size / 2, Quaternion.identity);
            
            // 攻撃範囲内の敵にダメージを与える
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<IEnemy>(out var enemy))
                {
                    enemy.TakeDamage(1);
                }
            }
            
            // クールタイム
            await UniTask.Delay(TimeSpan.FromSeconds(attackTime));
        }
        catch (Exception ex)
        {
            Debug.LogError("Error during attack: " + ex.Message);
        }
        finally
        {
            flickRangeObj.SetActive(false);
            _isAttacking = false;
        }
    }
}
}