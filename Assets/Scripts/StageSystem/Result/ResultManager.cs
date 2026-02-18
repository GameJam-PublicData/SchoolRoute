using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
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
    
    void Awake()
    {
        //シングルトン
        if (Instance == null || Instance != this)
        {
            Instance = this;
        }
        
        //コールバック登録
        goMainmenuButton.onClick.AddListener(GoMainMenu);
        resultPanel.gameObject.SetActive(false);
    }

    ISceneLoader _sceneLoader;
    
    [Inject]
    public void Construct(ISceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }
    
    void GoMainMenu()
    {
        Debug.Log("Go MainMenu");
        Time.timeScale = 1;
        _sceneLoader.LoadScene(SceneType.MainMenuScene);
    }

    public void Clear()
    {
        Time.timeScale = 0;
        resultPanel.gameObject.SetActive(true);
        winIcon.SetActive(true);
        loseIcon.SetActive(false);
        resultPanel.DOAnchorPosY(0, 1f).SetEase(Ease.OutBounce).SetUpdate(true);
    }

    public void Lose()
    {
        Time.timeScale = 0;
        resultPanel.gameObject.SetActive(true);
        winIcon.SetActive(false);
        loseIcon.SetActive(true);
        resultPanel.DOAnchorPosY(0, 1f).SetEase(Ease.OutBounce).SetUpdate(true);
    }
}
}
