using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoMain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SceneChange()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void gotoProlouge()
    {
        SceneManager.LoadScene("PrologueScene");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
