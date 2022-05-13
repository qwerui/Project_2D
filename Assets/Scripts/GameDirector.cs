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
    RectTransform gameOverScreenPos;

    private void Start() 
    {
        player = GameObject.Find("Player").GetComponent<PlayerContoller>().GetStat();
        healthText = GameObject.Find("HealthText").GetComponent<Text>();
        hpBar = GameObject.Find("HealthBar").GetComponent<Image>();
        hungerBar = GameObject.Find("HungerBar").GetComponent<Image>();
        gameOverScreenPos = GameObject.Find("GameOverScreen").GetComponent<Image>().GetComponent<RectTransform>();
        StartCoroutine("HealthTextController");
    }

    private void LateUpdate() {
        StatusController();
    }

    // 체력, 공복치 막대 업데이트
    private void StatusController()
    {
            // 주의) 나눗셈에서 float 캐스팅 안하면 소수점이 버려짐
            float hpPercent = player.getHp() / (float)player.getMaxHp();
            float hungerPercent = player.getHunger() / (float)player.getMaxHunger();

            hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, hpPercent, Time.deltaTime * 5);
            hungerBar.fillAmount = Mathf.Lerp(hungerBar.fillAmount, hungerPercent, Time.deltaTime * 5);
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
        StartCoroutine("ScreenDown");
    }
    public IEnumerator ScreenDown()
    {
        while(gameOverScreenPos.anchoredPosition.y > -180)
        {
            gameOverScreenPos.anchoredPosition = new Vector2(gameOverScreenPos.anchoredPosition.x, gameOverScreenPos.anchoredPosition.y-10);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("GameOverScene");
        yield return null;
    }
}
