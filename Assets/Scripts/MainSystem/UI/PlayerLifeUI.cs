using System;
using System.Collections.Generic;
using StageSystem.Player;
using UnityEngine;

namespace MainSystem.UI
{
public interface IPlayerLifeUI
{
    void SetMaxHP(float maxHP);
    void UpdateLifeUI(float currentHP);
}
public enum HeartState
{
    Full,
    Empty
}
public struct HeartData
{
    public HeartState State;
    public Sprite Sprite;
    public void SetState(HeartState state, Sprite sprite)
    {
        State = state;
        Sprite = sprite;
    }
}
public class PlayerLifeUI : MonoBehaviour, IPlayerLifeUI
{
   [SerializeField] LifeStateSO lifeStateSO;
   PlayerHPManager _playerHPManager;
   List<HeartData> _heartDataList = new ();
   
   float _maxHP;
   float _currentHP;

   public void SetMaxHP(float maxHP)
   {
       _maxHP = maxHP;
       for (int i = 0; i < _maxHP; i++)
       {
           _heartDataList.Add(new HeartData { State = HeartState.Full });
       }
   }

   public void UpdateLifeUI(float currentHP)
   {
       _currentHP = currentHP;
       for (int i = (int)_maxHP; i > 0 ;i--)
       {
           var heartData = _heartDataList[i-1];
           if (i <= _currentHP)
           {
               heartData.SetState(HeartState.Full, lifeStateSO.fullHeartSprite);
           }
           else
           { 
               heartData.SetState(HeartState.Empty, lifeStateSO.emptyHeartSprite);
           }
       }
   }
}
}