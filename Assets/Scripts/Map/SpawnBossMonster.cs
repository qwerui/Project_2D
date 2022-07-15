using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBossMonster : MonoBehaviour
{
    public GameObject bossSpawner;
    public GameObject nextStage;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player")
        {
            if (bossSpawner.transform.GetChild(0).childCount != 0)
                Invoke("BossSpawn", 1.0f);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Enemy")
        {
            nextStage.SetActive(true);
        }
    }
    void BossSpawn()
    {
        bossSpawner.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
    }
}
