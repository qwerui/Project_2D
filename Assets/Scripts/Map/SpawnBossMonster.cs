using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBossMonster : MonoBehaviour
{
    public GameObject bossSpawner;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player")
        {
            bossSpawner.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        }
    }
}
