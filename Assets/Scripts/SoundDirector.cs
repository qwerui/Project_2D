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
     */
    public AudioClip[] playerClip;
    /*
     * 플레이어 효과음 리스트
     * 0 : 이동
     * 1 : 점프
     * 2 : 공격
     * 3 : 피격
     * 4 : 사망
     * 5 : 대쉬
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
}
