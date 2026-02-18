using UnityEngine;
using DG.Tweening;

namespace StageSystem.Boss
{
public class ShotArea : MonoBehaviour
{
    [SerializeField] float MaxY = 1.5f;
    [SerializeField] float MinY = -1.5f;
    [SerializeField] float direction = 3f;

    void Start()
    {
        Move();
    }

    void Move()
    {
        transform.DOMoveY(MaxY, direction)
            .OnComplete(() => 
                transform.DOMoveY(MinY, direction)
                    .OnComplete(Move));
    }
}
}
