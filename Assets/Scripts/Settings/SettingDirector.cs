using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingDirector : MonoBehaviour
{
    public Toggle Tutorialtoggle;

    public GameObject LoginPanel;
    public GameObject LogoutPanel;
    
    public UploadManager uploadManager;

    public Text loginText;

    public GameObject serverObject;

    [Header("사운드 오브젝트")]
    public Slider BackgroundMusic;
    public Slider SoundFX;
    public AudioMixer mixer;

    bool isLogin;

    private void Start()
    {
        if(UserSessionCache.Instance.GetCredentials() != null)
        {
            ToggleLogInOut(true);
        }
        if (PlayerPrefs.GetInt("Tutorial", 1) == 1)
            Tutorialtoggle.isOn = true;
        else
            Tutorialtoggle.isOn = false;
        BackgroundMusic.value = PlayerPrefs.GetFloat("BGM", 0);
        SoundFX.value = PlayerPrefs.GetFloat("SoundFX",0);
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
    public void SetBackgroundMusic(float value)
    {
        mixer.SetFloat("BGM", value);
        PlayerPrefs.SetFloat("BGM", value);
    }
    public void SetSoundFX(float value)
    {
        mixer.SetFloat("SFX", value);
        PlayerPrefs.SetFloat("SoundFX", value);
    }
}
