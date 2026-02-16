using System;
using InputSystemActions;
using MainSystem.Scene;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;
namespace MainSystem
{
public class RootLifeTimeScope : LifetimeScope
{
    [SerializeField] bool loadBootSceneOnStart = true;
    protected override void Configure(IContainerBuilder builder)
    {
        // builder.Register<interface,class>();
        builder.Register<BootManager>(Lifetime.Singleton);
        builder.Register<ISceneLoader,SceneLoader>(Lifetime.Singleton);
        builder.Register<SceneInitializationAwaiter>(Lifetime.Singleton).AsImplementedInterfaces();
    }

    void Start()
    {
        var bootManager = Container.Resolve<BootManager>();
        bootManager.Initialize(loadBootSceneOnStart);
    }
}
}