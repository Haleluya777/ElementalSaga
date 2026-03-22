using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour, IDataInitializeable
{
    [SerializeField] private AudioMixer MasterVolume;
    [SerializeField] private List<AudioClip> BGMClips = new List<AudioClip>(); //BGM 오디오 파일.
    [SerializeField] private List<AudioClip> SEClips = new List<AudioClip>();  //효과음 오디오 파일.

    [SerializeField] private AudioSource BGMSources; //BGM 오디오 소스 컴포넌트.
    [SerializeField] private AudioSource SESources;  //SE 오디오 소스 컴포넌트.


    public void DataInitialize()
    {
        PlayBGM(0);
    }

    public void PlayBGM(int bgmNum)
    {
        BGMSources.clip = BGMClips[bgmNum];
        BGMSources.Play();
    }

    public void SetMasterVolume(float volume)
    {
        if (volume <= 0) MasterVolume.SetFloat("Master", -80);
        else MasterVolume.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetBGMVolume(float volume)
    {
        if (volume <= 0) MasterVolume.SetFloat("BGM", -80);
        else MasterVolume.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    public void SetSEVolume(float volume)
    {
        if (volume <= 0) MasterVolume.SetFloat("SE", -80);
        else MasterVolume.SetFloat("SE", Mathf.Log10(volume) * 20);
    }
}
