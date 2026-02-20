using DG.Tweening;
using UnityEngine;

namespace StageSystem.Player
{
public class PlayerForward : MonoBehaviour
{
    public Direction forwardDirection = Direction.Forward;
    public float HandAngle = 45f;

    public void ChangeForwardDirection(Direction newDirection,float fadeTime = 0.5f)
    {
        forwardDirection = newDirection;
        transform.DOLocalRotate(Quaternion.LookRotation(GetForwardVector(), Vector3.up).eulerAngles +new Vector3(HandAngle,0,180),fadeTime);
    }

    Vector3 GetForwardVector()
    {
        switch (forwardDirection)
        {
            case Direction.Forward:
                return Vector3.forward;
            case Direction.Backward:
                return Vector3.back;
            case Direction.Left:
                return Vector3.left;
            case Direction.Right:
                return Vector3.right;
            case Direction.Up  :
                return Vector3.up;
            case Direction.Down:
                return Vector3.down;
            default:
                return Vector3.forward;
        }
    }
}
}


