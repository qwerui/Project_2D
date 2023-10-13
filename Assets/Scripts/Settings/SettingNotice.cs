using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingNotice : MonoBehaviour
{
    public Text notice;
    public Button loginBtn;
    public Button signupBtn;
    //상태 팝업 창 출력
    public void SetNotice(string text)
    {
        notice.text = text;
        gameObject.SetActive(true);
        loginBtn.enabled = false;
        signupBtn.enabled = false;
    }
}
