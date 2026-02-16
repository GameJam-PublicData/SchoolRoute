using System;
using Cysharp.Threading.Tasks;
using InputSystemActions;
using MainSystem.Scene;

namespace MainSystem 
{
public class BootManager
{
    ISceneLoader _sceneLoader;
    public BootManager(ISceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }
    public void Initialize(bool isDebug)
    {
        // ゲームに必要な初期化処理

        // InputActionMapを全て有効化
        /*
        InputActions inputActions = new();
        inputActions.Enable();*/

        if (isDebug) return;
        
        _sceneLoader.LoadScene(SceneType.MainMenuScene).Forget();
    }
    
}
}