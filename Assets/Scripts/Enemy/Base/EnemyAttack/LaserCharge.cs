using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCharge : MonoBehaviour
{
    public GameObject beam;
    EnemyClass enemy;
    int damage;
    int soundIndex;

    public void ChargeLaser(int damage, EnemyClass enemy, int soundIndex)
    {
        this.enemy = enemy;
        this.soundIndex = soundIndex;
        enemy.PlaySound(soundIndex);
        this.damage = damage;
        gameObject.SetActive(true);
        Invoke("Fire",0.8f);
    }
    void Fire()
    {
        gameObject.SetActive(false);
        beam.SetActive(true);
        enemy.PlaySound(soundIndex+1);
        beam.GetComponent<Laser>().ShootBeam(this.damage);
    }
}
