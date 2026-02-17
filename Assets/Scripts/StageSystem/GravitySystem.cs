using System.Collections.Generic;
using UnityEngine;

namespace StageSystem
{
public interface IGravitySystem : IReadOnlyGravitySystem
{
    void ChangeGravity (Direction newDirection);
    
}
public interface IReadOnlyGravitySystem
{
    Direction GetGravityDirection();
    IReadOnlyDictionary<Direction, Vector3> OppositeDirections { get; }
}
public class GravitySystem : IGravitySystem
{
    Direction _gravityDirection = Direction.Down;
    
    public IReadOnlyDictionary<Direction, Vector3> OppositeDirections => new Dictionary<Direction, Vector3>
    {
        {Direction.Up, Vector3.up},
        {Direction.Right, Vector3.right},
        {Direction.Down, Vector3.down},
        {Direction.Left, Vector3.left},
        {Direction.Forward, Vector3.forward},
        {Direction.Backward, Vector3.back}
    };
    
    public Direction GetGravityDirection()
    {
        return _gravityDirection;
    }
    
    public void ChangeGravity(Direction newDirection)
    {
        _gravityDirection = newDirection;
    }
    
}
}