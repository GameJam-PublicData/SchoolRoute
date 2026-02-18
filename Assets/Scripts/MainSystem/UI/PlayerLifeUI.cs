using System.Collections.Generic;
using UnityEngine;

namespace MainSystem.UI
{
public interface IPlayerLifeUI
{
    void SetMaxHP(float maxHP);
    void UpdateLifeUI(float currentHP);
}
public enum LifeState
{
    Full,
    Empty
}
public struct HeartData
{
    GameObject _heartObj;
    LifeState _heartState;
    
    public HeartData(GameObject heartObj, LifeState lifeState)
    {
        _heartObj = heartObj;
        _heartState = lifeState;
    }
    
    public void ChangeState(LifeState lifeState)
    {
        _heartState = lifeState;
        // ※注意: 子オブジェクトにあるColorという名のオブジェクトを無理やり探すハードコーディングで実装してます
        _heartObj.transform.Find("Color").gameObject.SetActive(_heartState == LifeState.Full);
    }
}
public class PlayerLifeUI : MonoBehaviour, IPlayerLifeUI
{
    [Header("データ")]
    [SerializeField] GameObject _heartPrefab;
    
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
            
            // ハートの位置とスケールを設定
            rectTransform.anchoredPosition = position;
            rectTransform.localScale = Vector3.one * _heartScale;

            var heartData = new HeartData(heartObj, LifeState.Full);
            
            _heartDataList.Add(heartData);
        }
    }

    public void UpdateLifeUI(float currentHP)
    {
        _currentHP = currentHP;
        for (int i = 0; i < _heartDataList.Count; i++)
        {
            _heartDataList[i].ChangeState(i < _currentHP ? LifeState.Full : LifeState.Empty);
        }
    }
}
}