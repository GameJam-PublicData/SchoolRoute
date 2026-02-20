using System;
using DG.Tweening;
using StageSystem.Enemy;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StageSystem.Boss
{
public class Chalk : MonoBehaviour,IEnemy
{
    [SerializeField] GameObject _boss;
    ShotObject _shotObject;
    BossManager _bossManager;

    public void Init(GameObject boss,BossManager bossManager)
    {
        _boss = boss;
        _shotObject = boss.GetComponentInChildren<ShotObject>();
        _bossManager = bossManager;
    }
    
    void Start()
    {
        _shotObject = GetComponent<ShotObject>();
    }

    void FixedUpdate()
    {   
        if (transform.position.z < -4)
        {
            TakeDamage(100);
        }
    }
    
    public void TakeDamage(float damage)
    {
        _shotObject.MoveStop(); 
        
        //_bossの方向に自分がぶっ飛ぶ、イージングとかを使って
        //放物線を飛んでく　x+が縦軸

        float a = transform.position.x + 1.5f;
        transform.DOMoveX(a, 0.5f).SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                //ボスの高さまで
                transform.DOMoveX(_boss.transform.position.x, 0.5f)
                    .SetEase(Ease.InQuad);
            });
        
        transform.DOMoveZ(_boss.transform.position.z, 1f)
            .OnComplete(() =>
            {
                _bossManager.TakeDamage(1);
                Destroy(gameObject);
            });
        
        //めちゃくちゃ回転させる
        transform.DOLocalRotate(new Vector3(Random.Range(-1080, 1080), Random.Range(-1080, 1080), Random.Range(-1080, 1080)), 0.1f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }
}
}
