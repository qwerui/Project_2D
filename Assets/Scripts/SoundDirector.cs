using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundDirector : MonoBehaviour
{
    public AudioClip[] fxClip;
    /*
     * 효과음 리스트
     * 0 : UI 선택
     * 1 : UI 닫기
     * 2 : 다음 스테이지
     */
    public AudioClip[] playerClip;
    /*
     * 플레이어 효과음 리스트
     * 0 : 점프, 대쉬
     * 1 : 피격
     * 2 : 사망
     * 3 : 착지
     * 4 : 레벨업
     */
    AudioSource fx;
    AudioSource bgm;

    public AudioMixer mixer;
    private void Awake()
    {
        bgm = transform.GetChild(0).GetComponent<AudioSource>();
        fx = transform.GetChild(1).GetComponent<AudioSource>();
        mixer.SetFloat("BGM", PlayerPrefs.GetFloat("BGM", 0));
        mixer.SetFloat("SFX", PlayerPrefs.GetFloat("SoundFX", 0));
    }
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
