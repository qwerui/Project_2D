using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundDirector : MonoBehaviour
{
    public AudioClip[] fxClip;
    /*
     * ȿ���� ����Ʈ
     * 0 : UI ����
     * 1 : UI �ݱ�
     * 2 : ���� ��������
     */
    public AudioClip[] playerClip;
    /*
     * �÷��̾� ȿ���� ����Ʈ
     * 0 : ����, �뽬
     * 1 : �ǰ�
     * 2 : ���
     * 3 : ����
     * 4 : ������
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
