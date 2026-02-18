using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class UIAnimater : MonoBehaviour
{
    [SerializeField] private Sprite[] animationSprites;
    [SerializeField] private float animationInterval = 5/60f;
    
    private Image _image;
    private Coroutine _coroutine;

    private void Start()
    {
        _image = GetComponent<Image>();
        _coroutine = StartCoroutine(AnimationLoop());
    }
    
    private IEnumerator AnimationLoop()
    {
        //アニメーションループ
        while (true)
        {
            for (int i = 0; i < animationSprites.Length; i++)
            {
                _image.sprite = animationSprites[i];
                yield return new WaitForSecondsRealtime(animationInterval);

            }
        }
    }
    
    private void OnDestroy()
    {
        StopCoroutine(_coroutine);
    }
}
