 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverDirector : MonoBehaviour
{
    private void Update() {
        ReturnKeyPress();
    }
    void ReturnKeyPress()
    {
        if(Input.GetKeyDown(KeyCode.Return)||Input.GetKeyDown(KeyCode.KeypadEnter))
            ReturnMainMenu();
    }
    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }
}
