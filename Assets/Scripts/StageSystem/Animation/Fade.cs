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
    

    
     
    Image fadeImage;
    public void Init(Image fadeImage)
    { 
        this.fadeImage = fadeImage;
    }

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