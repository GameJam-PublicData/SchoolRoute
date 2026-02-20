using System.Collections;
using DG.Tweening;
using MainSystem.Audio;
using StageSystem.Player;
using UnityEngine;

namespace StageSystem.Enemy
{
public class Enemy : MonoBehaviour,IEnemy
{
    [SerializeField] float damage = 1f;

    [Header("マリオ状態だった時の敵だったらOn")]
    [SerializeField] bool isLine = false;
    
    /// <summary>
    /// ダメージを受けた時の処理
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void TakeDamage(float damage)
    {
        AudioManager.Instance.PlaySE("EnemyDamageSE");
        
        Burst();
    }

 
    void OnTriggerEnter(Collider collision)
    {
        var player = collision.GetComponentInParent<PlayerHPManager>();

        if (player != null)
        { 
            player.TakeDamage(damage);
        }
    }

    void Burst()
    {
        //リジッドボディとコライダーを消す
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<Collider>());
        
        //子オブジェクトでshadowという名前のオブジェクトがあったら消す
        var shadow = transform.Find("Shadow");
        if (shadow != null) Destroy(shadow.gameObject);
        
        //子オブジェクトでbodyという名前のオブジェクトを探す (z+ x+)
        var body = transform.Find("Body");
        if (body != null)
        {
            Vector3 targetPosition = body.transform.localPosition;
            
            if (isLine == false)
            {
                //z+の方向に吹っ飛ばす
                body.DOLocalMoveZ(body.transform.localPosition.z + 30f, 3f)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() => Destroy(gameObject));
            }
            else
            {
                //x+の方向に吹っ飛ばす
                body.DOLocalMoveX(body.transform.localPosition.x + 15f, 4f)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() => Destroy(gameObject));
            }
            
            body.transform.DOLocalMoveY(body.transform.localPosition.y + 5f,2)
                .SetLoops(-1,LoopType.Yoyo);
        }
    }
}
}
