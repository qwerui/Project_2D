using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleBoss : EnemyClass
{
    [SerializeField] GameObject laserObj;
    [SerializeField] GameObject bombPos;
    [SerializeField] GameObject bomb;
    LaserCharge laser;

    int laserDamage;
    int bombDamage;

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
        switch(pattern)
        {
            case 0:
                LaserAttack();
                break;
            case 1:
                isAttack = true;
                StartCoroutine("BombAttack");
                break;
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
        laser.ChargeLaser(laserDamage);
        Invoke("AttackEnd", 5.0f);
    }
    IEnumerator BombAttack()
    {
        int bombCount = 0;
        while(bombCount<5)
        {
            GameObject bombObj = Instantiate(bomb, bombPos.transform) as GameObject;
            bombObj.GetComponent<Bomb>().SetBomb(bombDamage, new Vector2(Random.Range(-5,-25),Random.Range(2,20)));
            bombCount++;
            yield return new WaitForSeconds(0.5f);
        }
        Invoke("AttackEnd", 2.0f);
        yield return null;
    }
    protected override void StatusInit()
    {
        hp = 10 * (data.stage + 10) + data.stage * data.stage;
        atk = data.stage * 20;
        def = (int)(data.stage / 5) * 3;
        experience = data.stage * 50;

        laserDamage = data.stage * 5 + 5;
        bombDamage = data.stage * 2 + 3;
    }
}
