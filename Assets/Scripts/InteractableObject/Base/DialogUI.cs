using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour //대화창
{
    string[] dialogText;
    public float textSpeed;
    int index = 0;
    Text textSpace;
    Vector3 pos;
    public AudioSource audioSource;
    public AudioClip[] clip;

    bool isTyping;

    //대화창 위치 결정 및 초기화
    public void DialogSetting(GameObject dialogObj, string[] dialogString)
    {
        pos = dialogObj.transform.position;
        dialogText = dialogString;
        textSpace = transform.GetChild(0).GetComponent<Text>();
        GoNextText();
    }
    //대화창 위치 조절
    private void FixedUpdate() {
        GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(pos+Vector3.up * 2.5f);
    }
    //다음 대화 출력
    public bool GoNextText()
    {
        StopAllCoroutines();
        if(index >= dialogText.Length)
        {
            Destroy(gameObject);
            return true;
        }
        else
        {
            StartCoroutine(TypingText(dialogText[index++]));
            StartCoroutine(TextSound());
            return false;
        } 
    }
    //타이핑 효과
    IEnumerator TypingText(string message)
    {
        isTyping = true;
        textSpace.text = "";
        for(int i=0;i<message.Length;i++)
        {
            textSpace.text += message[i];
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false;
    }
    //소리 출력
    IEnumerator TextSound()
    {
        while(isTyping)
        {
            audioSource.PlayOneShot(clip[0]);
            yield return new WaitForSeconds(0.875f);
            audioSource.PlayOneShot(clip[1]);
            yield return new WaitForSeconds(1.25f);
        }
    }
}
