using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class UIAnimater : MonoBehaviour
{
    [SerializeField] private Sprite[] animationSprites;
    [SerializeField] private float animationInterval = 5/60f;
    
    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
        AnimationLoop().Forget();
    }
    
    private async UniTask AnimationLoop()
    {
        //アニメーションループ
        while (true)
        {
            for (int i = 0; i < animationSprites.Length; i++)
            {
                _image.sprite = animationSprites[i];
                await UniTask.Delay((int)(animationInterval * 1000));
            }
        }
    }
}
