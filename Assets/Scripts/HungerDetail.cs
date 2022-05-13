using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerDetail : MonoBehaviour
{
    PlayerStatus player;
    Text hungerDetail;
    private void Start() {
        player = GameObject.Find("Player").GetComponent<PlayerContoller>().GetStat();
        hungerDetail = GetComponent<Text>();
    }
    private void LateUpdate() {
        hungerDetail.text = "Hunger : "+player.getHunger()+" / "+player.getMaxHunger();
    }
}
