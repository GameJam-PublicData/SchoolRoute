using DG.Tweening;
using MainSystem.Audio;
using MainSystem.Scene;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using UnityEngine.SceneManagement;

namespace MainMenu
{
public class MainMenuManager : MonoBehaviour
{
    //最初のボタン群
    [SerializeField] Button startButton;
    [SerializeField] Button licenseButton;
    
    //ライセンスオブジェクトの色々
    [SerializeField] RectTransform licenseTextObject;
    [SerializeField] Button licenseCloseButton;
    float _licenseTextDefaultX = -2000f;
    
    bool _canClick = true;
    

    void Start()
    {
        Init();
        
        AudioManager.Instance.PlayBGM("MainMenuBGM");
    }

    void Init()
    {
        //コールバックを登録
        startButton.onClick.AddListener(OnStartButton);
        licenseButton.onClick.AddListener(OnLicenseButton);
        licenseCloseButton.onClick.AddListener(OnLicenseCloseButton);
    }

    /// <summary>
    /// スタートボタンを押した時
    /// </summary>
    void OnStartButton()
    {
        
            AudioManager.Instance.PlaySE("ButtonSE");
            SceneManager.LoadScene("Stage1Scene");
    }
    
    /// <summary>
    /// ライセンスボタンを押した時
    /// </summary>
    void OnLicenseButton()
    {
        if (_canClick)
        {
            _canClick = false;
            licenseTextObject.anchoredPosition = new Vector2(_licenseTextDefaultX, licenseTextObject.anchoredPosition.y);
            licenseTextObject.DOAnchorPosX(0f,1f).SetEase(Ease.OutBack).OnComplete(() => _canClick = true);
            AudioManager.Instance.PlaySE("ButtonSE");
            
        }
    }

    /// <summary>
    /// ライセンスの閉じるボタンを押した時
    /// </summary>
    void OnLicenseCloseButton()
    {
        if (_canClick)
        {
            _canClick = true;
            licenseTextObject.anchoredPosition = new Vector2(0, licenseTextObject.anchoredPosition.y);
            licenseTextObject.DOAnchorPosX(_licenseTextDefaultX,1f).SetEase(Ease.OutBack).OnComplete(() => _canClick = true);
            AudioManager.Instance.PlaySE("ButtonSE");
        }
    }
}
}
