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
     */
    public AudioClip[] playerClip;
    /*
     * �÷��̾� ȿ���� ����Ʈ
     * 0 : �̵�
     * 1 : ����
     * 2 : ����
     * 3 : �ǰ�
     * 4 : ���
     * 5 : �뽬
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
