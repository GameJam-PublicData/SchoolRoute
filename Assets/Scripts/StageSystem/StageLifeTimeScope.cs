using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace StageSystem
{
public class StageLifeTimeScope : LifetimeScope
{
    [SerializeField] PlayerAnimationController _playerAnimationController;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_playerAnimationController);
        builder.Register<GravitySystem>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<PlayerAnimationController>(Lifetime.Singleton).AsImplementedInterfaces();
    }
}
}