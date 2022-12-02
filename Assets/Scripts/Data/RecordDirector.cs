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

    public GameObject uploadButton;
    public DynamoDBManager DynamoDB;

    void Start()
    {
        localRank = new LocalRanking();
        localRank = JsonDirector.LoadRanking();
        localRankObj.SetActive(true);

        UserSessionCache session = UserSessionCache.Instance;
        if(session.GetCredentials() == null) //업로드 버튼 활성화(로그인 시)
        {
            uploadButton.SetActive(false);
        }
        else
        {
            uploadButton.SetActive(true);
        }
        LoadLocal();
        LoadWorld();
    }
    //랭킹 화면 전환
    public void RecordLoading(int option)
    {
        RecordObjInit();
        if (option == 0)
        {
            localRankObj.SetActive(true); //로컬 랭킹
        }
        else if(option == 1)
        {
            worldRankObj.SetActive(true); //서버 랭킹
        }
    }
    //로컬 랭킹 불러오기
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
    //월드 랭킹 불러오기
    void LoadWorld()
    {
        List<WorldRank> worldRankList = DynamoDB.LoadRanking();
        worldRankList.Sort((a, b) => b.Score.CompareTo(a.Score)); //내림차순
        for(int i=0;i<worldRankList.Count;i++)
        {
            GameObject rankingLine = Instantiate(worldRankPrefab, worldRankObj.transform.GetChild(0).GetChild(0));
            rankingLine.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -(i * 25));
            rankingLine.transform.GetChild(0).GetComponent<Text>().text = (i+1).ToString();
            rankingLine.transform.GetChild(1).GetComponent<Text>().text = worldRankList[i].Username;
            rankingLine.transform.GetChild(2).GetComponent<Text>().text = worldRankList[i].Score.ToString();
        }
    }
    //랭킹 오브젝트 초기화
    void RecordObjInit()
    {
        localRankObj.SetActive(false);
        worldRankObj.SetActive(false);
    }
    //업로드 상태 출력
    public void UploadRanking()
    {
        if(DynamoDB.UploadRanking())
        {
            uploadButton.transform.GetChild(0).GetComponent<Text>().text = "upload success";
        }
        else
        {
            uploadButton.transform.GetChild(0).GetComponent<Text>().text = "upload fail";
        }
        Invoke("UploadText",1.0f);
    }
    void UploadText()
    {
        uploadButton.transform.GetChild(0).GetComponent<Text>().text = "upload ranking";
    }
}
