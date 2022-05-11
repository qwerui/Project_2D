using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : EnemyClass
{
    private int nextMove;

    private void Awake() {
        Think();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate() {
        PlatformCheck();
        Move();    
    }
    public override void Attack()
    {

    }
    public override void Move()
    {
        if(nextMove != 0)
        {
            isMoving = true;
            transform.localScale = new Vector3(nextMove,1,1);
        }
        else
        {
            isMoving=false;
        }
        rigid.velocity= new Vector2(moveSpeed*nextMove, 0);
        ani.SetBool("Run", isMoving);
    }
    void Think(){
        nextMove = Random.Range(-1,2);
        float time = Random.Range(2f, 5f);
        Invoke("Think", time); //매개변수로 받은 함수를 time초의 딜레이를 부여하여 재실행 
    }
    void PlatformCheck()
    {
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down*1.5f, new Color(0,1,0));
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down,1.5f,groundLayer);
        if(raycast.collider == null){
            nextMove= nextMove*(-1); 
            CancelInvoke(); //think를 잠시 멈춘 후 재실행
            Invoke("Think",5); 
        }
    }       
} 
