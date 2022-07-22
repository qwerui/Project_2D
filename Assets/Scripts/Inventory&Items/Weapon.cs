using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    PlayerStatus stat;
    private void OnTriggerEnter2D(Collider2D other) {
        stat = transform.parent.gameObject.GetComponent<PlayerController>().GetStat();
        other.GetComponent<EnemyClass>().Hit(stat.getAtk());
    }
}
