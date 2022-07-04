using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCharge : MonoBehaviour
{
    public GameObject beam;
    int damage;

    public void ChargeLaser(int damage)
    {
        this.damage = damage;
        gameObject.SetActive(true);
        Invoke("Fire",0.8f);
    }
    void Fire()
    {
        gameObject.SetActive(false);
        beam.SetActive(true);
        beam.GetComponent<Laser>().ShootBeam(this.damage);
    }
}
