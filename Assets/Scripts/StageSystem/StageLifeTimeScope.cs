using StageSystem.Animation;
using StageSystem.Player;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace StageSystem
{
public class StageLifeTimeScope : LifetimeScope
{
    [SerializeField] PlayerAnimationController playerAnimationController;

    [SerializeField] Image FadeImage;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(playerAnimationController);
        builder.Register<PlayerAnimationController>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<IGravitySystem, GravitySystem>(Lifetime.Singleton).As<IReadOnlyGravitySystem>();
        builder.Register<IFade, Fade>(Lifetime.Singleton).AsImplementedInterfaces();
    }

    void Start()
    {
        var fade = Container.Resolve<IFade>();
        fade.Init(FadeImage);
    }
}
}