namespace StageSystem
{
public interface IGravitySystem : IReadOnlyGravitySystem
{
    void ChangeGravity (Direction newDirection);
}
public interface IReadOnlyGravitySystem
{
    Direction GetGravityDirection();
}
public class GravitySystem : IGravitySystem
{
    Direction _gravityDirection = Direction.Down;
    
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