 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverDirector : MonoBehaviour
{
    DataDirector data;
    bool activeReturn = false;
    
    /*
        0: 스테이지
        1: 플레이어 레벨
        2: 적 처치 횟수
        3: 자원획득 수
        4: 최종 점수
    */
    float[] score = new float[5];
    int[] scoreEnd = new int[5];
    public Text[] scoreText = new Text[5];
    float countingDuration = 1f;
    public GameObject returnObj;

    private void Start() {
        data = GameObject.Find("DataDirector").GetComponent<DataDirector>();
        for(int i=0;i<score.Length;i++)
        {
            score[i]=0;
        }
        ScoreInit();
        StartCoroutine("ScorePrint");
    }
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
        if(activeReturn)
        {
            data.stage=1;
            data.level=1;
            data.resourceItem=0;
            data.enemySlain=0;
            SceneManager.LoadScene("MainScene");
        }
    }
    IEnumerator ScorePrint()
    {
        for(int i=0;i<score.Length;i++)
        {
            scoreText[i].transform.parent.gameObject.SetActive(true);
            float offset = (scoreEnd[i] - score[i]) / countingDuration;
            while(score[i] < scoreEnd[i])
            {
                score[i] += offset * Time.deltaTime;
                scoreText[i].text = ((int)score[i]).ToString();
                yield return null;
            }
            scoreText[i].text = scoreEnd[i].ToString();
        }
        yield return new WaitForSeconds(countingDuration);
        returnObj.SetActive(true);
        activeReturn=true;
    }
    void ScoreInit()
    {
        int sum = 1;

        scoreEnd[0] = data.stage;
        scoreEnd[1] = data.level;
        scoreEnd[2] = data.enemySlain;
        scoreEnd[3] = data.resourceItem;
        for(int i=0;i<scoreEnd.Length-1;i++)
        {
            sum*=scoreEnd[i];
        }
        scoreEnd[4] = (int)(sum / 10000);
    }
}
