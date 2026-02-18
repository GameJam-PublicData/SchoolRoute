using VContainer;
using VContainer.Unity;

namespace StageSystem
{
public class StageLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<IGravitySystem, GravitySystem>(Lifetime.Singleton).As<IReadOnlyGravitySystem>();
    }
}
}