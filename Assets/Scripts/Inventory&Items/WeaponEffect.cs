using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEffect : MonoBehaviour
{
    PlayerStatus stat;
    //무기 충돌 판정
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag=="Enemy")
        {
            stat = transform.parent.gameObject.GetComponent<PlayerController>().GetStat();
            other.GetComponent<EnemyClass>().Hit(stat.getAtk());
        }
    }
}
