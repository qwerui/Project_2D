using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundPopup : MonoBehaviour //게임 플레이 중 소리 조절
{
    public Slider bgm;
    public Slider fx;

    public AudioMixer mixer;
    public void GetCurrentVolume()
    {
        bgm.value = PlayerPrefs.GetFloat("BGM", 0);
        fx.value = PlayerPrefs.GetFloat("SoundFX", 0);
    }
    public void SetBackgroundMusic(float value)
    {
        mixer.SetFloat("BGM", value);
        PlayerPrefs.SetFloat("BGM", value);
    }
    public void SetSoundFX(float value)
    {
        mixer.SetFloat("SFX", value);
        PlayerPrefs.SetFloat("SoundFX", value);
    }
}
