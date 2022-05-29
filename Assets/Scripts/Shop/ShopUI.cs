using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
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
