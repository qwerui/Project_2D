using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuDirector : MonoBehaviour
{
    int sceneIndex;
    [SerializeField] GameObject arrow;
    RectTransform arrowPos;

    void Start()
    {
        arrowPos = arrow.GetComponent<RectTransform>();
        sceneIndex = 0;
    }

    private void Update() {
        GetKeyboard();
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


    public void StartBtnDown()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void SetBtnDown()
    {
        SceneManager.LoadScene("SettingScene");

    }

    public void RecBtnDown()
    {
        SceneManager.LoadScene("RecordScene");

    }
}
