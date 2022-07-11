using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingDirector : MonoBehaviour
{
    public Toggle Tutorialtoggle;

    public GameObject LoginPanel;
    public GameObject LogoutPanel;
    
    public UploadManager uploadManager;

    public Text loginText;

    public GameObject serverObject;

    bool isLogin;

    private void Start()
    {
        if(UserSessionCache.Instance.GetCredentials() != null)
        {
            ToggleLogInOut(true);
        }
        if (PlayerPrefs.GetInt("Tutorial") == 1)
            Tutorialtoggle.isOn = true;
        else
            Tutorialtoggle.isOn = false;
    }
    public void TutorialSetting(bool toggle)
    {
        if (toggle)
            PlayerPrefs.SetInt("Tutorial", 1);
        else
            PlayerPrefs.SetInt("Tutorial", 0);
    }
    public void ToggleLogInOut(bool status)
    {
        if(status)
        {
            isLogin=true;
            loginText.text = "logout";
            uploadManager.SetDyanmoDB(true);
            serverObject.SetActive(true);
        }
        else
        {
            isLogin=false;
            loginText.text = "login";
            uploadManager.SetDyanmoDB(false);
            serverObject.SetActive(false);
        }
    }
    public void ShowLogInOutPanel()
    {
        if(isLogin)
        {
            LogoutPanel.SetActive(true);
        }
        else
        {
            LoginPanel.SetActive(true);
        }
    }
}
