using System;
using Cysharp.Threading.Tasks;
using StageSystem.Animation;
using UnityEngine;
using VContainer;

namespace StageSystem.Player
{
public class PlayerHealth : MonoBehaviour
{
    PlayerHPManager _hpManager;
    PlayerMover _mover;
    IFade _fade;
    ICountdownManager _countdown;

    GameObject grandParent;
    Vector3 respawnPoint;
    
    
    [Inject]
    void Init(IFade fade, ICountdownManager countdown)
    { 
        _fade = fade;
        _countdown = countdown;
    }

    void Start()
    {
        grandParent = transform.parent.parent.gameObject;
        
        _hpManager = grandParent.GetComponent<PlayerHPManager>();
        _mover = grandParent.GetComponent<PlayerMover>();
    }

    async void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if(other.gameObject.CompareTag("SavePoint"))
        {
            // ! 座標の保存
            // セーブポイントの更新
            respawnPoint = _mover.transform.position;
            Debug.Log("Save Point Updated: " + respawnPoint);
        }
        if(other.gameObject.CompareTag("DeathZone"))
        {
            Debug.Log("Entered Death Zone!");
            
            _hpManager.TakeDamage(1f);
            _mover.canMove = false;
            
            var duration = 1.5f;
            
            // フェードイン
            _fade.FadeIn(duration, () => _mover.ResetPosition(respawnPoint));
            
            // 待機
            await UniTask.Delay(TimeSpan.FromSeconds(duration + 0.5)); // TODO: フェードインの完了を正確に待つ
            
            // フィードアウト
            _fade.FadeOut(duration, () => _countdown.StartCountdown(() => _mover.canMove = true));
        }
    }
}
}