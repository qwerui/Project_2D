using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameDirector : MonoBehaviour
{
    PlayerStatus player;
    Text healthText;
    Image hpBar;
    Image hungerBar;

    Text redBall;
    Text blueBall;
    Text yellowBall;
    Text atk;
    Text def;
    Text gold;
    Text SavePopupText;

    [SerializeField] GameObject[] Objects;
    /*
        0~2  빨, 파, 노 구슬
        3~4  공격력, 방어력
        5    골드 숫자
        6    인벤토리
        7    플레이어
        8~10 상태창 (체력 숫자, 체력 바, 공복치 바)
        11   게임오버 화면 전환을 위한 이미지
        12   세이브 팝업
        23   RoomController
    */

    private void Awake() {
        redBall = Objects[0].GetComponent<Text>();
        blueBall = Objects[1].GetComponent<Text>();
        yellowBall = Objects[2].GetComponent<Text>();
        atk = Objects[3].GetComponent<Text>();
        def = Objects[4].GetComponent<Text>();
        gold = Objects[5].GetComponent<Text>();
        SavePopupText = Objects[12].transform.GetChild(0).GetComponent<Text>();
    }
    private void Start() 
    {
        Objects[6].SetActive(false);
        player = Objects[7].GetComponent<PlayerController>().GetStat();
        healthText = Objects[8].GetComponent<Text>();
        hpBar = Objects[9].GetComponent<Image>();
        hungerBar = Objects[10].GetComponent<Image>();
        StartCoroutine("HealthTextController");
    }

    private void LateUpdate() {
        StatusController();
        InventoryTextController();
    }

    // 체력, 공복치 막대 업데이트
    private void StatusController()
    {
            // 주의) 나눗셈에서 float 캐스팅 안하면 소수점이 버려짐
            float hpPercent = player.getHp() / (float)player.getMaxHp();
            float hungerPercent = player.getHunger() / (float)player.getMaxHunger();

            hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, hpPercent, Time.deltaTime * 5);
            hungerBar.fillAmount = Mathf.Lerp(hungerBar.fillAmount, hungerPercent, Time.deltaTime * 5);

            hpBar.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Hp : "+player.getHp()+" / "+player.getMaxHp();
            hungerBar.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Hunger : "+player.getHunger()+" / "+player.getMaxHunger();
    }
    private IEnumerator HealthTextController()
    {
        while(true)
        {
            int health = Int32.Parse(healthText.text);
            if(player.getHp() > health)
                healthText.text = (health+1).ToString();
            else if(player.getHp() < health)
                healthText.text = (health-1).ToString();

            yield return new WaitForSeconds(0.05f);
        }
    }
    
    public void GameOver()
    {
        Objects[11].SetActive(true); //게임 오버 스크린 활성화
        Objects[11].transform.GetChild(0).gameObject.SetActive(true);//게임오버 텍스트 활성화
        StartCoroutine("GameOverFadeIn");
    }

    public IEnumerator GameOverFadeIn()
    {
        Image sr;
        Text tx;

        sr = Objects[11].GetComponent<Image>();
        tx = Objects[11].transform.GetChild(0).gameObject.GetComponent<Text>();

        for(int i=0;i<100;i++)
        {
            float f = i / 100.0f;

            var tempColor = sr.color;
            tempColor.a = f;
            sr.color = tempColor;

            tempColor = tx.color;
            tempColor.a = f;
            tx.color = tempColor;
            
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("GameOverScene");
        yield return null;
    }

    public void InventoryTextController()
    {
        redBall.text = player.getRedBall().ToString();
        blueBall.text = player.getBlueBall().ToString();
        yellowBall.text = player.getYellowBall().ToString();
        gold.text = player.getGold().ToString();
        atk.text = "ATK : "+player.getAtk().ToString();
        def.text = "DEF : "+player.getDef().ToString();
    }
    public void ShowSavePopup()
    {
        if(Objects[13].GetComponent<RoomController>().GetRoomInfo(DataDirector.Instance.playerPosIndex).roomType == RoomType.Boss)
        {
            SavePopupText.text = "보스 방에서는 저장할 수 없습니다";
            Objects[12].transform.GetChild(1).gameObject.SetActive(false);
            Objects[12].transform.GetChild(2).gameObject.SetActive(false);
            Invoke("CloseSavePopup", 0.5f);
        }
        else
        {
            SavePopupText.text = "게임을 저장하고 메인화면으로 돌아가시겠습니까?";
            Objects[12].transform.GetChild(1).gameObject.SetActive(true);
            Objects[12].transform.GetChild(2).gameObject.SetActive(true);
        }
        Objects[12].SetActive(true);
    }
    void CloseSavePopup()
    {
        Objects[12].SetActive(false);
    }
}
