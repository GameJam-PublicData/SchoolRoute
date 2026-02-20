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
    PlayerJumpManager _jumpManager;
    YMover _yMover;

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
        _hpManager = GetComponentInChildren<PlayerHPManager>();
        _mover = GetComponentInChildren<PlayerMover>();
        _jumpManager = GetComponentInChildren<PlayerJumpManager>();
        _yMover = GetComponentInChildren<YMover>();
        Debug.Log($"_yMover found: {_yMover != null}");
    }

    bool FallChack = true;
    void Update()
    {
        if(Vector3.Distance(_jumpManager.transform.position,transform.position) < 5f) return;
        {
            if(!FallChack) return;
            _hpManager.TakeDamage(1);
        }
    }

    public void PlayerDamaged()
    {
       
        FallChack = false;
        Debug.LogError("Fell off the stage!");
        _mover.canMove = false;
        _jumpManager.StopJump();
            
        var duration = 1.5f;
        // フェードイン
        _yMover.SetInputEnabled(false);
        
        _fade.FadeIn(duration, () => _mover.ResetPosition(respawnPoint));
        UniTask.Delay(TimeSpan.FromSeconds(duration + 0.5)).ContinueWith(() =>
        {
            Debug.Log("Jumping to the last checkpoint...+" + _mover.nextRouteIndex);
            Debug.Log( _mover.transform.position = _mover.stageRouteSO.RouteDataList[_mover.nextRouteIndex - 1].TargetPosition);
            if (_mover.stageRouteSO.RouteDataList[_mover.nextRouteIndex - 2].JumpTargetPosition != Vector3.zero)
            {
                _mover.transform.position = _mover.stageRouteSO.RouteDataList[_mover.nextRouteIndex - 2].JumpTargetPosition;
            }
            else _mover.transform.position = _mover.stageRouteSO.RouteDataList[_mover.nextRouteIndex - 2].TargetPosition;
            _mover.nextRouteIndex -= 2;
            _jumpManager.transform.localPosition = Vector3.zero;
            _mover.UpdateRouteData(true);
                
         
              
        });
        // 待機
        UniTask.Delay(TimeSpan.FromSeconds(duration + 0.5)).ContinueWith(() =>
        {
            // フィードアウト
            _fade.FadeOut(duration, () => _countdown.StartCountdown(() =>
            {
                FallChack = true;
                _mover.canMove = true;
                _jumpManager.ResetJump();
                _yMover.SetInputEnabled(true);
            })); 
        });

    }

    async void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        /*
        if(other.gameObject.CompareTag("SavePoint"))
        {
            // ! 座標の保存
            // セーブポイントの更新
            respawnPoint = _mover.transform.position;
            Debug.Log("Save Point Updated: " + respawnPoint);
        }*/
        /*
        if(other.gameObject.CompareTag("DeathZone"))
        {
            Debug.Log("Entered Death Zone!");
            
            _hpManager.TakeDamage(1);
            _mover.canMove = false;
            _mover.CurrentRouteIndex -= 1;
            
            var duration = 1.5f;
            
            // フェードイン
            _fade.FadeIn(duration, () => _mover.ResetPosition(respawnPoint));
            
            // 待機
            await UniTask.Delay(TimeSpan.FromSeconds(duration + 0.5)); // TODO: フェードインの完了を正確に待つ
            
            // フィードアウト
            _fade.FadeOut(duration, () => _countdown.StartCountdown(() => _mover.canMove = true));
        }*/
    }
}
}