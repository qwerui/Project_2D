using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    string[] dialogText;
    public float textSpeed;
    int index = -1;
    Text textSpace;

    public void DialogSetting(GameObject dialogObj, string[] dialogString)
    {
        GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(dialogObj.transform.position+Vector3.up * 2.2f);
        dialogText = dialogString;
        textSpace = transform.GetChild(0).GetComponent<Text>();
        GoNextText();
    }

    public bool GoNextText()
    {
        StopAllCoroutines();
        StartCoroutine(TypingText(dialogText[++index]));
        if(index >= dialogText.Length)
        {
            Destroy(gameObject);
            return true;
        }
        else
            return false;
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
