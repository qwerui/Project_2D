using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DebugScript : MonoBehaviour
{
    //단순 디버깅용 스크립트 의미없는 코드들
    public AudioSource sound;
    public AudioClip clip;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
            StartCoroutine(SoundPlay());
    }
    IEnumerator SoundPlay()
    {
        for (int i = 0; i < 4; i++)
        {
            sound.pitch = 0.8f - (i * 0.2f);
            sound.volume = 0.6f - (i * 0.1f);
            sound.PlayOneShot(clip);
            yield return new WaitForSeconds(0.25f);
        }
    }
}
