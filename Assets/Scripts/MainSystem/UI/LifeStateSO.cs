using UnityEngine;

namespace MainSystem.UI
{
[CreateAssetMenu(fileName = "LifeStateSO", menuName = "ScriptableObjects/LifeStateSO", order = 1)]
public class LifeStateSO : ScriptableObject
{
    [SerializeField] public Sprite fullHeartSprite;
    [SerializeField] public Sprite emptyHeartSprite;
}
}