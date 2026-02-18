using System;
using UnityEngine;
using DG.Tweening;

namespace StageSystem
{
public interface ICameraSystem
{
    public void SetRotation(Vector3 targetDir,Vector3 localPos);
}

public class CameraSystem : MonoBehaviour, ICameraSystem
{
    Transform _rootObject;

    [Header("カメラの動作")] [SerializeField] float rotationSpeed = 1f;

    [Header("DOTWEEN")] [SerializeField] Ease easeType = Ease.InOutSine;
    

    void Awake()
    {
        _rootObject = transform.parent;
    }

    Vector3 _targetDir = Vector3.zero;
    public void SetRotation(Vector3 targetDir, Vector3 localPos)
    {
        _targetDir = targetDir;
        
        transform.DOLocalMove(localPos, rotationSpeed).SetEase(easeType);
        
        /*
        DOTween.To(() => _rotation,
            x => _rotation = x, 
            rotation,
            rotationSpeed
            ).SetEase(easeType);*/
        // カメラの位置と回転を移動
        /*
        Sequence seq = DOTween.Sequence();
        seq.SetAutoKill(true);
        seq.Append(); // TODO: playerを中心に回転させるようにする*/
        //seq.Join(transform.DORotateQuaternion(targetRot, rotationSpeed).SetEase(easeType));
    }

    void Update()
    {
        _rootObject.rotation = Quaternion.identity;
        transform.LookAt(_rootObject.transform.position + _targetDir);

       
    }
}
}
