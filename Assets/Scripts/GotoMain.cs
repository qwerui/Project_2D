using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoMain : MonoBehaviour
{
    //단순한 화면 이동
    public void SceneChange()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void gotoProlouge()
    {
        SceneManager.LoadScene("PrologueScene");
    }
}
