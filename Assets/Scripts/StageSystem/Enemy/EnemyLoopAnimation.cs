using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyAnimationLoop : MonoBehaviour
{
    [SerializeField] private EnemyAnimation animation;
    
    void Start()
    {
        AnimationLoop().Forget();
    }

    private async UniTask AnimationLoop()
    {
        while (true)
        {
            await animation.Animate();
            
            if(this == null) break;
        }
    }
}
