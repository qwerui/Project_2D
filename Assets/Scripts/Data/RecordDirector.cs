using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordDirector : MonoBehaviour
{
    LocalRanking localRank;
    [Header("∑©≈∑¿ª ≥÷¿ª ø¿∫Í¡ß∆Æ")]
    public GameObject localRankObj;
    public GameObject worldRankObj;
    [Header("∑©≈∑ «— ¡Ÿ «¡∏Æ∆’")]
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
            GameObject rankingLine = Instantiate(localRankPrefab, localRankObj.transform);
            rankingLine.transform.position = new Vector2(0, i * 20);
            rankingLine.transform.GetChild(0).GetComponent<Text>().text = i.ToString();
            rankingLine.transform.GetChild(1).GetComponent<Text>().text = localRank.score[i].ToString();
        }
    }
    void RecordObjInit()
    {
        localRankObj.SetActive(false);
        worldRankObj.SetActive(false);
    }

}
