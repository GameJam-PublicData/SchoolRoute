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
    
    Vector3 respawnPoint;
    
    [Inject]
    void Init(IFade fade, ICountdownManager countdown)
    { 
        _fade = fade;
        _countdown = countdown;
    }

    void Start()
    {
        var parentObj = transform.parent.gameObject;
        _hpManager = parentObj.GetComponent<PlayerHPManager>();
        _mover = parentObj.GetComponent<PlayerMover>();
    }

    async void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if(other.gameObject.CompareTag("SavePoint"))
        {
            // セーブポイントの更新
            respawnPoint = transform.position;
        }
        if(other.gameObject.CompareTag("DeathZone"))
        {
            Debug.Log("Entered Death Zone!");
            
            _hpManager.TakeDamage(1f);
            _mover.canMove = false;
            
            var duration = 1.5f;
            
            // フェードイン・フェードアウトとプレイヤーの移動
            _fade.FadeIn(duration, MoveRespawnPoint);
            await UniTask.Delay(TimeSpan.FromSeconds(duration + 0.5));
            _fade.FadeOut(duration, () => _countdown.StartCountdown(() => _mover.canMove = true));
        }
    }

    void MoveRespawnPoint()
    {
        Debug.Log("Moving to respawn point: " + respawnPoint);
        transform.position = respawnPoint;
    }
}
}