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
    public AudioSource sfxAudio;

    private void Start() {
        data = DataDirector.Instance;
        for(int i=0;i<score.Length;i++)
        {
            score[i]=0;
        }
        ScoreInit();
        SaveLocalRanking();
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
            int soundCount = 0;
            
            while(score[i] < scoreEnd[i])
            {
                score[i] += offset * Time.deltaTime;
                scoreText[i].text = ((int)score[i]).ToString();
                if((int)score[i]>soundCount)
                {
                    sfxAudio.PlayOneShot(sfxAudio.clip);
                    soundCount++;
                }
                yield return null;
            }
            scoreText[i].text = scoreEnd[i].ToString();
            yield return new WaitForSeconds(countingDuration);
        }
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
        scoreEnd[4] = (int)(sum / 1000);
    }
    void SaveLocalRanking()
    {
        LocalRanking localRank = new LocalRanking();
        localRank = JsonDirector.LoadRanking();
        if(localRank.score == null)
        {
            localRank.score = new int[10];
            localRank.rank = new int[10];
        }

        int[] localScore = new int[11];
        int[] rankNum = new int[10];
        for(int i=0;i<10;i++)
        {
            rankNum[i]=i+1;
            localScore[i]=localRank.score[i];
        }
        localScore[10] = scoreEnd[4];
        System.Array.Sort(localScore);
        System.Array.Reverse(localScore);
        System.Array.Resize(ref localScore,10);

        localRank.rank = rankNum;
        localRank.score = localScore;

        JsonDirector.SaveRanking(localRank);
    }
}
