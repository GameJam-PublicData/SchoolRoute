
using UnityEngine;

namespace StageSystem
{
public interface ICameraSystem
{
    public void SetRotation(Direction direction);
}
public class CameraSystem : MonoBehaviour, ICameraSystem
{
    public void SetRotation(Direction direction)
    {
        
    }
}
}
