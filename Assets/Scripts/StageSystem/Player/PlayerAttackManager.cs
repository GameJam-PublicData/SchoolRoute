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
            flickRangeObj.SetActive(true);
            var sphere = flickRangeObj.GetComponent<Collider>();
            var hits = Physics.OverlapSphere(sphere.bounds.center, sphere.bounds.extents.x);
            
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<IEnemy>(out var enemy))
                {
                    enemy.TakeDamage(1);
                }
            }
            
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