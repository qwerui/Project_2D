using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuDirector : MonoBehaviour
{
    int sceneIndex;
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject TutorialPopup;
    RectTransform arrowPos;

    void Start()
    {
        arrowPos = arrow.GetComponent<RectTransform>();
        sceneIndex = 0;
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
                LoadGame();
        }
    }

    public void StartBtnDown()
    {
        TutorialPopup.SetActive(true);
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
        SceneManager.LoadScene("GameScene");
    }
}
