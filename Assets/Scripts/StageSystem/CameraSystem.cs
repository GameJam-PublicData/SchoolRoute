using System;
using UnityEngine;
using DG.Tweening;

namespace StageSystem
{
public interface ICameraSystem
{
    public void SetRotation(Direction direction);
}
public class CameraSystem : MonoBehaviour, ICameraSystem
{
    Transform _rootObject;
    
    [Header("カメラの動作")]
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] float distance = 10f;
    
    [Header("DOTWEEN")]
    [SerializeField] Ease easeType = Ease.InOutSine;

    void Start()
    {
        _rootObject = transform.parent;
        transform.position = _rootObject.position - _rootObject.forward * distance;
    }

    public void SetRotation(Direction direction)
    {
        Vector3 targetDir = direction switch
        {
            Direction.Up       => Vector3.down,
            Direction.Left     => Vector3.right,
            Direction.Right    => Vector3.left,
            Direction.Forward  => Vector3.back,
            Direction.Backward => Vector3.forward,
            _                  => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Unsupported direction.")
        };
        
        Vector3 targetPos = _rootObject.position - targetDir * distance;
        Quaternion targetRot = Quaternion.LookRotation(_rootObject.position - targetPos);
    
        // カメラの位置と回転を移動
        Sequence seq = DOTween.Sequence();
        seq.SetAutoKill(true);
        seq.Append(transform.DOMove(targetPos, rotationSpeed).SetEase(easeType)); // TODO: playerを中心に回転させるようにする
        seq.Join(transform.DORotateQuaternion(targetRot, rotationSpeed).SetEase(easeType));
    }
}
}
