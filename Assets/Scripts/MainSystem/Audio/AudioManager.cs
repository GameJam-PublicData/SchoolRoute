using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace MainSystem.Audio
{
/// <summary> オーディオの大まかなカテゴリ </summary>
public enum AudioCategory
{
    Master,
    BGM,
    SE,
    /// <summary> SEより長いサウンド </summary>
    Jingle
}

//3D以外のオーディオ管理を行うマネージャークラス
//SE、BGM、ボイスなど
public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioSO audioSO;
    
    AudioSource _bgmSource;
    List<AudioSource> _seSources = new();
    
    const int StartSESourceCount = 4;

    CancellationTokenSource _bgmFadCTS;
    
    void Awake()
    {
        _bgmSource = CreateAudioSource("BGM", true);
        
        for (int i = 0; i < StartSESourceCount; i++)
        {
            _seSources.Add(CreateAudioSource("SE", false));
        }
    }
    
    AudioSource CreateAudioSource(string mixerGroup, bool loop)
    {
        var source = gameObject.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = audioMixer.FindMatchingGroups(mixerGroup)[0];
        source.loop = loop;
        source.spatialBlend = 0f; // 2D
        return source;
    }
    
    public async UniTask PlayBGM(string bgmKey, float fadeTime = 0f)
    {
        AudioClip clip = audioSO.BGMSounds.Find(s => s.SoundName == bgmKey)?.Clip;
        if (clip == null) return;
        
        audioMixer.GetFloat("BGMVolume", out float mixerVolume);

        _bgmFadCTS?.Cancel();
        if (fadeTime != 0)
        {
            fadeTime /= 2f;//フェードアウトとフェードインで分割
        }
        
        if (_bgmSource.isPlaying)
        {
            //BGMが再生中ならフェードアウト
            if (fadeTime > 0f)
            {
                _bgmFadCTS = new CancellationTokenSource();
                await FadeOutBGM(fadeTime, _bgmFadCTS);
            }
            else
            {
                _bgmSource.volume = mixerVolume;
                _bgmSource.Stop();
            }
        }
        //新しいBGMをセットしてフェードイン
        if (fadeTime > 0f)
        {
            _bgmSource.volume = 0f;
            _bgmSource.clip = clip;
            _bgmSource.Play();
            _bgmSource.DOFade(mixerVolume, fadeTime);
            await  UniTask.Delay((int)(fadeTime * 1000));
        }
        else
        {
            _bgmSource.clip = clip;
            _bgmSource.Play();
        }
        
    }
    
    public void PlaySE(string seKey, float volume = 1f)
    {
        AudioClip clip = audioSO.SESounds.Find(s => s.SoundName == seKey)?.Clip;
        if (clip == null) return;

        AudioSource source = GetAvailable2DSource();
        source.PlayOneShot(clip, volume);
    }
    
    public void StopBGM(float fadeTime = 0f)
    {
        _bgmFadCTS?.Cancel();
        if (fadeTime > 0f)
        {
            _bgmFadCTS = new CancellationTokenSource();
            FadeOutBGM(fadeTime, _bgmFadCTS).Forget();
        }
        else
        {
            _bgmSource.Stop();
        }
    }

    public void SetVolume(AudioCategory category, float volume)
    {
        string paramName = $"{category}Volume";
        audioMixer.SetFloat(paramName, VolumeToDb(volume));
    }


    AudioSource GetAvailable2DSource()
    {
        foreach (var source in _seSources)
        {
            if (!source.isPlaying)
                return source;
        }
        
        var newSource = CreateAudioSource("SE", false);
        _seSources.Add(newSource);
        return newSource;
    }
    
    //BGMをフェードアウトさせる
    async UniTask FadeOutBGM(float duration,CancellationTokenSource cts,float fadeEndVolume = 0f)
    {
        if (audioMixer.GetFloat("BGMVolume", out float mixerVolume) == false)
        {
            throw new Exception("BGMVolumeパラメータが見つかりません");
        }
        try
        {
            _bgmSource.DOKill();
            _bgmSource.DOFade(fadeEndVolume, duration);
        
            await UniTask.Delay((int)(duration * 1000), cancellationToken: cts.Token);
        }
        finally
        {
            _bgmSource.Stop();
            _bgmSource.volume = mixerVolume;
        }
    }

    float VolumeToDb(float volume)
    {
        return volume > 0 ? 20f * Mathf.Log10(volume) : -80f;
    }
    void OnDestroy()
    {
        _bgmFadCTS?.Cancel();
    }
}
}