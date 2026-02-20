using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace StageSystem.Boss
{
public class BossAnimation : MonoBehaviour
{
    [SerializeField] float AnimationTime = 0.5f;
    
    [SerializeField] List<GameObject> headObjects;
    [SerializeField] float headAnimationMovement = 0.5f;
    
    [SerializeField] List<GameObject> bodyObjects;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BossAnimationFunction();
    }

    void BossAnimationFunction()
    {
        //headObjectsは、headAnimationmovement分、上下する,イージング使用
        foreach (var headObject in headObjects)
        {
            headObject.transform.DOLocalMoveY(headObject.transform.position.y + headAnimationMovement, AnimationTime)
                .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
        
        //bodyObjectsは大きさが xを0.8倍　zを1.2倍にする,イージング使用
        foreach (var bodyObject in bodyObjects)
        {
            bodyObject.transform.DOScaleX(bodyObject.transform.localScale.x * 0.8f, AnimationTime)
                .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            bodyObject.transform.DOScaleZ(bodyObject.transform.localScale.z * 1.2f, AnimationTime)
                .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
    }
}
}
