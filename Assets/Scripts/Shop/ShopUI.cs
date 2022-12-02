using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    //상점에서 현재 골드의 양을 보여주는 클래스
    [SerializeField] Text playerGold;
    public void SetPlayerGold(int gold)
    {
        playerGold.text = gold+"￦";
    }
    public void ResumeTime()
    {
        Time.timeScale = 1;
    }
}
