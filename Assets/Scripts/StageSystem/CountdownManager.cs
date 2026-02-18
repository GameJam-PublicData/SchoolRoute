using UnityEngine;
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace StageSystem
{
public interface ICountdownManager
{
    void StartCountdown(Action onCompleted = null);
}
public class CountdownManager : MonoBehaviour, ICountdownManager
{
    [Header("配置")]
    [SerializeField] Vector3 basePosition = new (0, 0, 0);
    
    [Header("データ")]
    [SerializeField] List<GameObject> countdownSteps;
    [SerializeField] GameObject goObject;
    
    public async void StartCountdown(Action onCompleted = null)
    {
        foreach (var step in countdownSteps)
        {
            await InstanceShowStepObject(step);
        }
        
        InstanceShowStepObject(goObject, 1.25f).Forget();
        onCompleted?.Invoke();
    }

    async UniTask InstanceShowStepObject(GameObject step, float showTime = 1f)
    {
        var instance = Instantiate(step, transform);
        instance.transform.localPosition = basePosition;
        await UniTask.Delay(TimeSpan.FromSeconds(showTime));
        Destroy(instance);
    }
}
}