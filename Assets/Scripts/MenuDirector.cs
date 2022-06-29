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
    RectTransform arrowPos;

    bool onTutorial;
    void Start()
    {
        arrowPos = arrow.GetComponent<RectTransform>();
        sceneIndex = 0;
        if (PlayerPrefs.GetInt("Tutorial") == 1)
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
        if(Input.GetKeyDown(KeyCode.DownArrow))
            if(sceneIndex < 2)
                sceneIndex++;
        if(Input.GetKeyDown(KeyCode.UpArrow))
            if(sceneIndex > 0)
                sceneIndex--;
        if(Input.GetKeyDown(KeyCode.Return)||Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if(sceneIndex == 0)
                StartBtnDown();
            else if(sceneIndex == 1)
                SetBtnDown();
            else if(sceneIndex == 2)
                RecBtnDown();
        }
        
    }

    void GetKeyPopup()
    {
        if(TutorialPopup.activeSelf == true)
        {
            if(Input.GetKeyDown(KeyCode.Y))
                LoadTutorial();
            else if(Input.GetKeyDown(KeyCode.N))
                GameStart();
        }
        if(LoadGamePopup.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.Y))
                LoadGame();
            else if (Input.GetKeyDown(KeyCode.N))
                GameStart();
        }
    }

    public void StartBtnDown()
    {
        if (onTutorial)
            TutorialPopup.SetActive(true);
        else
        {
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

    public void SetBtnDown()
    {
        SceneManager.LoadScene("SettingScene");

    }

    public void RecBtnDown()
    {
        SceneManager.LoadScene("RecordScene");

    }
    public void LoadTutorial()
    {
        SceneManager.LoadScene("PrologueScene");
    }
    public void LoadGame()
    {
        DataDirector.Instance.isLoadedGame = true;
        GameStart();
    }
    public void GameStart()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void NotGoTutorial()
    {
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
