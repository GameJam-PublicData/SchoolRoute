using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;

namespace StageSystem.Animation
{
public interface IFade
{
    void Init(Image fadeImage);
    void FadeIn(float duration = 1, Action onCompleted = null);
    void FadeOut(float duration = 1, Action onCompleted = null);
}
public class Fade : IFade
{
    Image _fadeImage;
    
    public void Init(Image fadeImage) => _fadeImage = fadeImage;

    public void FadeIn(float duration = 1, Action onCompleted = null)
    {
        _fadeImage.color = new Color(0, 0, 0, 0);
        _fadeImage.DOFade(1, duration).OnComplete(() => onCompleted?.Invoke());
    }

    public void FadeOut(float duration = 1, Action onCompleted = null)
    {
        _fadeImage.color = new Color(0, 0, 0, 1);
        _fadeImage.DOFade(0, duration).OnComplete(() => onCompleted?.Invoke());
    }
}
}