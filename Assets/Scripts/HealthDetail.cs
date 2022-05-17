using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDetail : MonoBehaviour
{
    PlayerStatus player;
    [SerializeField] GameObject playerObject;
    Text healthDetail;
    private void Start() {
        player = playerObject.GetComponent<PlayerContoller>().GetStat();
        healthDetail = GetComponent<Text>();
    }
    private void LateUpdate() {
        healthDetail.text = "Hp : "+player.getHp()+" / "+player.getMaxHp();
    }
}
