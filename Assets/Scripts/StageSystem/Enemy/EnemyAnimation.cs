using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

//エネミーの子オブジェクトとして、アニメーションごとにオブジェクトを作り、それぞれにこのスクリプトをアタッチする

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] Sprite[] animationSprites;
    [SerializeField] private float animationInterval = 5/60f;
    
    private SpriteRenderer _enemySpriteRenderer;

    void Awake()
    {
        //親のスプライトレンダラーを取得
        _enemySpriteRenderer = GetComponentInParent<SpriteRenderer>();
        Debug.Log(_enemySpriteRenderer);
    }

    public async UniTask Animate()
    {
        for(int i = 0;i < animationSprites.Length;i++)
        {
            _enemySpriteRenderer.sprite = animationSprites[i];
            await UniTask.Delay((int)(animationInterval * 1000));
        }
    }
}
