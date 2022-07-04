using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordDirector : MonoBehaviour
{
    LocalRanking localRank;
    [Header("랭킹 위치 오브젝트")]
    public GameObject localRankObj;
    public GameObject worldRankObj;
    [Header("랭킹 프리팹")]
    public GameObject localRankPrefab;
    public GameObject worldRankPrefab;
    void Start()
    {
        localRank = new LocalRanking();
        localRank = JsonDirector.LoadRanking();
        LoadLocal();
    }

    public void RecordLoading(int option)
    {
        RecordObjInit();
        if (option == 0)
        {
            localRankObj.SetActive(true);
        }
    }
    void LoadLocal()
    {
        for(int i=0;i<10;i++)
        {
            GameObject rankingLine = Instantiate(localRankPrefab, localRankObj.transform.GetChild(0).GetChild(0));
            rankingLine.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -(i * 25)-20);
            rankingLine.transform.GetChild(0).GetComponent<Text>().text = (i+1).ToString();
            rankingLine.transform.GetChild(1).GetComponent<Text>().text = localRank.score[i].ToString();
        }
    }
    void RecordObjInit()
    {
        localRankObj.SetActive(false);
        worldRankObj.SetActive(false);
    }
    void ContentChange(int index)
    {
        if(index == 0)
        {
            localRankObj.SetActive(true);
            worldRankObj.SetActive(false);
        }
        else if(index == 1)
        {
            localRankObj.SetActive(false);
            worldRankObj.SetActive(true);
        }
    }
}
