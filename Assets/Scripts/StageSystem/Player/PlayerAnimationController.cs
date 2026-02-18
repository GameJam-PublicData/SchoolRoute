using System;
using UnityEngine;

namespace StageSystem.Player
{
public interface IPlayerAnimationController
{
    void PlayerWalk();
    float PlayerAttack();// 攻撃のアニメーションを再生し、攻撃のタイミングを返す
    void PlayerDamaged();
    void PlayerDeath();
}
public class PlayerAnimationController : MonoBehaviour ,IPlayerAnimationController
{
    [SerializeField]  Animator animator;

    void Reset()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayerWalk()
    {
        animator.Play("Walk");
    }
    public float PlayerAttack ()
    {
        animator.Play("Decopin");
        return 0.2f;//攻撃のタイミングを返す（例: アニメーションの0.3秒後に攻撃が当たる）
    }

    public void PlayerDamaged()
    {
        Debug.LogError("PlayerDamaged animation not implemented yet!");
    }

    public void PlayerDeath()
    {
        Debug.LogError("PlayerDeath animation not implemented yet!");
    }

    void Start()
    {
        PlayerWalk();
    }
}
}