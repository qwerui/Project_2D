using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingDirector : MonoBehaviour
{
    public Toggle Tutorialtoggle;
    private void Start()
    {

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
}
