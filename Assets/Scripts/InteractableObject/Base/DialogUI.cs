using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    string[] dialogText;
    public float textSpeed;
    int index = 0;
    Text textSpace;
    Vector3 pos;

    public void DialogSetting(GameObject dialogObj, string[] dialogString)
    {
        pos = dialogObj.transform.position;
        dialogText = dialogString;
        textSpace = transform.GetChild(0).GetComponent<Text>();
        GoNextText();
    }
    private void FixedUpdate() {
        GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(pos+Vector3.up * 2.2f);
    }

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
            return false;
        } 
    }
    IEnumerator TypingText(string message)
    {
        textSpace.text = "";
        for(int i=0;i<message.Length;i++)
        {
            textSpace.text += message[i];
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
