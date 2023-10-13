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
    public Text resolutionText;

    public GameObject serverObject;

    public Slider BackgroundMusic;
    public Slider SoundFX;
    public AudioMixer mixer;

    bool isLogin;

    readonly string[] resolution = {"640x360", "1280x720"}; //화면 크기
    int resolutionIndex = 0;

    private void Start() //각종 설정 불러오기
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
        resolutionIndex = PlayerPrefs.GetInt("Resolution",0);
        resolutionText.text = resolution[resolutionIndex];
    }
    //튜토리얼 활성화
    public void TutorialSetting(bool toggle)
    {
        if (toggle)
            PlayerPrefs.SetInt("Tutorial", 1);
        else
            PlayerPrefs.SetInt("Tutorial", 0);
    }
    //로그인,로그아웃 토글
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
    //로그인,로그아웃 팝업 출력
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
    //소리 조절
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
    //화면 크기 조절
    public void SetResolution(bool isRight)
    {
        if(isRight)
        {
            resolutionIndex++;
        }
        else
        {
            resolutionIndex--;
        }
        resolutionIndex = Mathf.Clamp(resolutionIndex,0,resolution.Length-1);
        string[] screenSize = resolution[resolutionIndex].Split('x');
        resolutionText.text = resolution[resolutionIndex];
        Screen.SetResolution(int.Parse(screenSize[0]),int.Parse(screenSize[1]),FullScreenMode.Windowed,0);
        PlayerPrefs.SetInt("Resolution",resolutionIndex);
    }
}
