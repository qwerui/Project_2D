using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundDirector : MonoBehaviour
{
    public AudioClip[] fxClip;
    //효과음
    public AudioClip[] playerClip;
    //플레이어 효과음
    AudioSource fx;
    AudioSource bgm;

    public AudioMixer mixer;
    private void Awake()
    {
        bgm = transform.GetChild(0).GetComponent<AudioSource>();
        fx = transform.GetChild(1).GetComponent<AudioSource>();
        //볼륨 설정
        mixer.SetFloat("BGM", PlayerPrefs.GetFloat("BGM", 0));
        mixer.SetFloat("SFX", PlayerPrefs.GetFloat("SoundFX", 0));
    }
    //효과음 출력
    public void FxPlay(int index)
    {
        fx.PlayOneShot(fxClip[index]);
    }
    public void PlayerSoundPlay(int index)
    {
        fx.PlayOneShot(playerClip[index]);
    }
    public void FxPlayWithClip(AudioClip clip)
    {
        fx.PlayOneShot(clip);
    }
    public void SetFxPitch(float pitch)
    {
        fx.pitch = pitch;
    }
    public void SetFxVolume(float volume)
    {
        fx.volume = volume;
    }
}
