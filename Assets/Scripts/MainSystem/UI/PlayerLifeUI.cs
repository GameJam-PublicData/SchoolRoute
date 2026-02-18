using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainSystem.UI
{
public interface IPlayerLifeUI
{
    void SetMaxHP(float maxHP);
    void UpdateLifeUI(float currentHP);
}
public struct HeartData
{
    Image _heartImage;
    LifeStateSO _lifeStateSO;
    
    public HeartData(Image image, LifeStateSO initialState)
    {
        _heartImage = image;
        _lifeStateSO = initialState;
        _heartImage.sprite = initialState._stateSprite;
    }
    
    public void ChangeState(LifeStateSO lifeStateSO)
    {
        _lifeStateSO = lifeStateSO;
        _heartImage.sprite = _lifeStateSO._stateSprite;
    }
}
public class PlayerLifeUI : MonoBehaviour, IPlayerLifeUI
{
    [Header("データ")]
    [SerializeField] GameObject _heartPrefab;
    [SerializeField] LifeStateSO _fullHeartSO;
    [SerializeField] LifeStateSO _emptyHeartSO;
    
    [Header("配置")]
    [SerializeField] Vector3 _heartStartPosition = new (-850, 425, 0);
    [SerializeField] float _heartSpacing = 75f;
    [SerializeField] float _heartScale = 0.85f;
    List<HeartData> _heartDataList = new ();
   
    float _maxHP;
    float _currentHP;

    public void SetMaxHP(float maxHP)
    {
        _maxHP = maxHP;
        for (int i = 0; i < _maxHP; i++)
        {
            // ハートの位置を調整
            Vector3 position = _heartStartPosition + new Vector3(i * _heartSpacing, 0, 0);
            
            var heartObj = Instantiate(_heartPrefab, transform);
            
            var rectTransform = heartObj.GetComponent<RectTransform>();
            var heartImage = heartObj.GetComponent<Image>();
            
            // ハートの位置とスケールを設定
            rectTransform.anchoredPosition = position;
            rectTransform.localScale = Vector3.one * _heartScale;
            
            var heartData = new HeartData(heartImage, _emptyHeartSO);
            heartData.ChangeState(_fullHeartSO);
            
            _heartDataList.Add(heartData);
        }
    }

    public void UpdateLifeUI(float currentHP)
    {
        _currentHP = currentHP; 
        for (int i = 0; i < _heartDataList.Count; i++)
        {
            _heartDataList[i].ChangeState(i < _currentHP ? _fullHeartSO : _emptyHeartSO);
        }
    }
}
}