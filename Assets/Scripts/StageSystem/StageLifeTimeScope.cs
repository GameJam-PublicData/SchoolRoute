using StageSystem.Animation;
using MainSystem.UI;
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
    [SerializeField] PlayerLifeUI playerLifeUI;
    [SerializeField] CountdownManager countdownManager;

    [SerializeField] Image FadeImage;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(playerLifeUI).As<IPlayerLifeUI>();
        builder.RegisterInstance(playerAnimationController);
        builder.Register<PlayerAnimationController>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<IGravitySystem, GravitySystem>(Lifetime.Singleton).As<IReadOnlyGravitySystem>();
        builder.Register<IFade, Fade>(Lifetime.Singleton);
        builder.RegisterInstance(countdownManager).As<ICountdownManager>();
    }

    void Start()
    {
        var fade = Container.Resolve<IFade>();
        if (fade != null)
        {
            Debug.Log("Fade instance resolved successfully.");
        }
        else
        {
            Debug.LogError("Failed to resolve IFade instance.");
        }
        fade.Init(FadeImage);
    }
}
}