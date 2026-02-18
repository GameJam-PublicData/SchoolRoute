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
        // Player関連は時オブジェクトから取得
        _hpManager = GetComponent<PlayerHPManager>();
        _mover = GetComponent<PlayerMover>();
        
        _fade = fade;
        _countdown = countdown;
    }
    
    async void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("SavePoint"))
        {
            // セーブポイントの更新
            respawnPoint = transform.position;
        }
        if(other.gameObject.CompareTag("DeathZone"))
        {
            Debug.Log("Entered Death Zone!");
            
            _hpManager.TakeDamage(1f);
            _mover.ChangeCanMove(false);
            
            var duration = 1.5f;
            
            // フェードイン・フェードアウトとプレイヤーの移動
            _fade.FadeIn(duration, MoveRespawnPoint);
            await UniTask.Delay(TimeSpan.FromSeconds(duration + 0.5));
            _fade.FadeOut(duration, () => _countdown.StartCountdown());
            
            _mover.ChangeCanMove(true);
        }
    }

    void MoveRespawnPoint()
    {
        Debug.Log("Moving to respawn point: " + respawnPoint);
        transform.position = respawnPoint;
    }
}
}