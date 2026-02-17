using UnityEngine;
using System;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace StageSystem.Animation
{
public class Fade : MonoBehaviour
{
    [SerializeField] Image fadeImage;

    public void FadeIn(float duration = 1, Action onCompleted = null)
    {
        fadeImage.color = new Color(0, 0, 0, 0);
        fadeImage.DOFade(1, duration).OnComplete(() => onCompleted?.Invoke());
    }

    public void FadeOut(float duration = 1, Action onCompleted = null)
    {
        fadeImage.color = new Color(0, 0, 0, 1);
        fadeImage.DOFade(0, duration).OnComplete(() => onCompleted?.Invoke());
    }
}
}