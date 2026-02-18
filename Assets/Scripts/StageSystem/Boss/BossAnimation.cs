using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace StageSystem.Boss
{
public class BossAnimation : MonoBehaviour
{
    [SerializeField] List<GameObject> headParts; // 頭、目、メガネ、フレームのGameObjectを格納するリスト

    [SerializeField] float breathingAmplitude = 0.1f;  // 呼吸の振幅
    [SerializeField] float breathingFrequency = 0.75f; // 呼吸の周波数
    [SerializeField] Ease breathingEase = Ease.InOutSine;                                           // 呼吸のイージング

    [SerializeField] List<GameObject> bodyParts; // 体系
    
    
    void Start()
    {
        BreathingAnimation();
    }
    
    //呼吸で頭が上下するアニメーション
    public void BreathingAnimation()
    {
        //頭、目、メガネ、フレームを上下させる DOTWeenでBreathingAmplitude分上下させる
        foreach (var bodyPart in headParts)
        {
            bodyPart.transform.DOLocalMoveY(bodyPart.transform.localPosition.y + breathingAmplitude, 1f / breathingFrequency)
                .SetEase(breathingEase)
                .SetLoops(-1, LoopType.Yoyo);
        }

        //体を動かす
        //sizeのx0.8 z1.2にする
        foreach (var bodyPart in bodyParts)
        {
            bodyPart.transform.DOScaleX(bodyPart.transform.localScale.x * 0.8f, 1f / breathingFrequency)
                .SetEase(breathingEase)
                .SetLoops(-1, LoopType.Yoyo);
            bodyPart.transform.DOScaleZ(bodyPart.transform.localScale.z * 1.2f, 1f / breathingFrequency)
                .SetEase(breathingEase)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}
}