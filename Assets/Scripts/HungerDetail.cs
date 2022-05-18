using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerDetail : MonoBehaviour
{
    PlayerStatus player;
    [SerializeField]GameObject playerObject;
    Text hungerDetail;
    private void Start() {
        player = playerObject.GetComponent<PlayerController>().GetStat();
        hungerDetail = GetComponent<Text>();
    }
    private void LateUpdate() {
        hungerDetail.text = "Hunger : "+player.getHunger()+" / "+player.getMaxHunger();
    }
}
