using System;
using UnityEngine;
using DG.Tweening;

namespace StageSystem
{
public interface ICameraSystem
{
    public void SetRotation(Direction direction,float rotation,float distance);
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
    float _rotation = 0f;
    Direction _currentDirection;
    public void SetRotation(Direction direction,float rotation,float distance)
    {
        _rotation = 0f;
        _currentDirection = direction;
        Debug.Log($"CameraSystem: SetRotation called with direction {direction}");
        Vector3 targetDir = direction switch
        {
            Direction.Up       => Vector3.down,
            Direction.Down     => Vector3.up,
            Direction.Left     => Vector3.left,
            Direction.Right    => Vector3.right,
            Direction.Forward  => Vector3.back,
            Direction.Backward => Vector3.forward,
            _                  => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Unsupported direction.")
        };

        Vector3 targetPos = targetDir * (-1 * distance);

        transform.DOLocalMove(targetPos, rotationSpeed).SetEase(easeType);
        
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
        transform.LookAt(_rootObject);
        switch (_currentDirection)
        {
            case Direction.Up:
            case Direction.Down:
                transform.Rotate(0, 0,_rotation);
                break;
            case Direction.Left:
            case Direction.Right:
                transform.Rotate(0,_rotation,0);
                break;
            case Direction.Forward:
            case Direction.Backward:
                transform.Rotate(0,_rotation,0);
                break;
        }
        //transform.Rotate(0, _rotation, 0);

       
    }
}
}
