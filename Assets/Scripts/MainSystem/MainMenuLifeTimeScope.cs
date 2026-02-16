using System;
using MainSystem.Scene;
using VContainer;
using VContainer.Unity;

namespace MainSystem
{
public class MainMenuLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // MainMenuSceneに特化した依存関係の登録をここに追加
    }

    void Start()
    {
        // MainMenuSceneの初期化処理をここに追加
        var pub =  Container.Resolve<ISceneInitializationPublisher>();
        pub.NotifyInitializationComplete();
    }
}
}