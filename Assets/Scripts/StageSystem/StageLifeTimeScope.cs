using StageSystem.Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace StageSystem
{
public class StageLifeTimeScope : LifetimeScope
{
    [SerializeField] PlayerAnimationController playerAnimationController;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(playerAnimationController);
        builder.Register<PlayerAnimationController>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<IGravitySystem, GravitySystem>(Lifetime.Singleton).As<IReadOnlyGravitySystem>();
    }
}
}