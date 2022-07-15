using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuDirector : MonoBehaviour
{
    int sceneIndex;
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject TutorialPopup;
    [SerializeField] GameObject LoadGamePopup;
    [SerializeField] SoundDirector sound;
    RectTransform arrowPos;

    bool onTutorial;
    bool PopupOn;
    void Start()
    {
        arrowPos = arrow.GetComponent<RectTransform>();
        sceneIndex = 0;
        if (PlayerPrefs.GetInt("Tutorial", 1) == 1)
            onTutorial = true;
        else
            onTutorial = false;
    }

    private void Update() {
        GetKeyboard();
        GetKeyPopup();
    }
    private void FixedUpdate() {
        ArrowMove();
    }
    void ArrowMove()
    {
        arrowPos.anchoredPosition = new Vector2(arrowPos.anchoredPosition.x, -160-60*sceneIndex);
    }

    void GetKeyboard()
    {
        if(!PopupOn)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
                if (sceneIndex < 2)
                {
                    sceneIndex++;
                    sound.FxPlay(0);
                }
            if (Input.GetKeyDown(KeyCode.UpArrow))
                if (sceneIndex > 0)
                {
                    sceneIndex--;
                    sound.FxPlay(0);
                }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (sceneIndex == 0)
                    StartBtnDown();
                else if (sceneIndex == 1)
                    SetBtnDown();
                else if (sceneIndex == 2)
                    RecBtnDown();
            }
        }
    }

    void GetKeyPopup()
    {
        if(TutorialPopup.activeSelf == true)
        {
            if(Input.GetKeyDown(KeyCode.Y))
                LoadTutorial();
            else if(Input.GetKeyDown(KeyCode.N))
            {
                NotGoTutorial();
            }
                
        }
        else if(LoadGamePopup.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.Y))
                LoadGame();
            else if (Input.GetKeyDown(KeyCode.N))
            {
                GameStart();
            }
                
        }
    }

    public void StartBtnDown()
    {
        PopupOn = true;

        if (onTutorial)
        {
            TutorialPopup.SetActive(true);
            sound.FxPlay(0);
        }
        else
        {
            if(JsonDirector.CheckSaveFile())
            {
                sound.FxPlay(0);
                LoadGamePopup.SetActive(true);
            }
            else
            {
                GameStart();
            }
        }
    }

    public void SetBtnDown()
    {
        sound.FxPlay(0);
        SceneManager.LoadScene("SettingScene");
    }

    public void RecBtnDown()
    {
        sound.FxPlay(0);
        SceneManager.LoadScene("RecordScene");
    }
    public void LoadTutorial()
    {
        sound.FxPlay(0);
        SceneManager.LoadScene("PrologueScene");
    }
    public void LoadGame()
    {
        sound.FxPlay(0);
        DataDirector.Instance.isLoadedGame = true;
        SceneManager.LoadScene("GameScene");
    }
    public void GameStart()
    {
        sound.FxPlay(0);
        DataDirector.Instance.isLoadedGame = false;
        SceneManager.LoadScene("GameScene");
    }
    public void NotGoTutorial()
    {
        sound.FxPlay(0);
        TutorialPopup.SetActive(false);
        if(JsonDirector.CheckSaveFile())
        {
            LoadGamePopup.SetActive(true);
        }
        else
        {
            GameStart();
        }
    }
}
