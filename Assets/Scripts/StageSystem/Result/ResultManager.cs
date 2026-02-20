using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MainSystem.Audio;
using MainSystem.Scene;
using UnityEngine.Serialization;
using VContainer;

namespace StageSystem.Result
{
public class ResultManager : MonoBehaviour
{
    public static ResultManager Instance;
    [SerializeField] RectTransform resultPanel;
    [SerializeField] Button goMainmenuButton;

    [SerializeField] GameObject winIcon;
    [SerializeField] GameObject loseIcon;
    [SerializeField] GameObject Canvas;

    void Start()
    {
        //シングルトン
        if (Instance == null || Instance != this)
        {
            Instance = this;
        }
        

    }

    ISceneLoader _sceneLoader;
    
    [Inject]
    public void Construct(ISceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
        //コールバック登録
        goMainmenuButton.onClick.AddListener(GoMainMenu);
        resultPanel.gameObject.SetActive(false);
    }
    
    public void GoMainMenu()
    {
        Debug.Log("Go MainMenu");
        Time.timeScale = 1;
        AudioManager.Instance.PlaySE("ButtonSE");
        if(_sceneLoader == null)
        {
            Debug.LogError("SceneLoader is not injected in ResultManager.");
            return;
        }
        _sceneLoader.LoadScene(SceneType.MainMenuScene).Forget();
    }

    public void Clear()
    {
        AudioManager.Instance.PlaySE("ClearSE");
        Time.timeScale = 0;
        resultPanel.gameObject.SetActive(true);
        winIcon.SetActive(true);
        loseIcon.SetActive(false);
        Canvas.SetActive(false);
        resultPanel.DOAnchorPosY(0, 1f).SetEase(Ease.OutBounce).SetUpdate(true);
    }

    public void Lose()
    {
        AudioManager.Instance.PlaySE("LoseSE");
        Time.timeScale = 0;
        resultPanel.gameObject.SetActive(true);
        winIcon.SetActive(false);
        loseIcon.SetActive(true);
        resultPanel.DOAnchorPosY(0, 1f).SetEase(Ease.OutBounce).SetUpdate(true);
    }
}
}
