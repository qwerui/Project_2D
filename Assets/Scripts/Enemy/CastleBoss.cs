using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleBoss : EnemyClass
{
    [SerializeField] GameObject laserObj;
    LaserCharge laser;

    void Start()
    {
        EnemyInit();
        laser = laserObj.GetComponent<LaserCharge>();
        StartCoroutine("PosDiff");
    }

    private void FixedUpdate() 
    {
        if(!isDead)
        {
            if(!isAttack)
                Attack();
        }
    }

    protected override void Attack()
    {
        int pattern = Random.Range(0,2);
        if(pattern==0)
        {
            LaserAttack();
        }
    }
    
    protected override void Move()
    {

    }
    protected override void Think()
    {

    }
    protected override void Chase()
    {

    }
    protected override IEnumerator PosDiff()
    {
        while(true)
        {
            posDiff = transform.position.x - player.transform.position.x;
            yield return new WaitForSeconds(1f);
        }
    }

    void LaserAttack()
    {
        isAttack = true;
        laser.ChargeLaser();
        Invoke("AttackEnd", 5.0f);
    }
}
