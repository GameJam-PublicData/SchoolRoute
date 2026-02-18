using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    readonly Animator _animator;

    public PlayerAnimationController(Animator animator)
    {
        _animator = animator;
    }
    
    public void PlayerWalk()
    {
        _animator.Play("PlayerWalk");
    }
    public void PlayerAttack ()
    {
        _animator.Play("Attack");
    }
}
