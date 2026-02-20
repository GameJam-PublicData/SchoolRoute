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
        animator.Play("Damage");
    }

    public void PlayerDeath()
    {
        animator.Play("Dead");
    }

    void Start()
    {
        PlayerWalk();
    }
}
}