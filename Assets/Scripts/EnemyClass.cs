using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyClass : MonoBehaviour
{
    public int hp;
    public int atk;
    public int def;
    public float moveSpeed;
    public Animator ani;
    public Rigidbody2D rigid;
    public LayerMask groundLayer;
    public BoxCollider2D hitbox;
    public GameObject player;

    protected int nextMove;
    protected float posDiff;

    protected bool isMoving;

    public abstract void Move();
    public abstract void Attack();
    public abstract void Think();
    public abstract void Chase();
    public abstract IEnumerator PosDiff();

    public void Hit()
    {
        ani.SetTrigger("Hit");
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        
        if(collision.gameObject.tag == "Player"){
            player.GetComponent<PlayerContoller>().Damaged(this.transform.position);
        }

    }
    public bool SetOnChase(float radius)
    {
        if(Physics2D.OverlapCircle((Vector2)transform.position + hitbox.offset,radius,LayerMask.GetMask("Player")))
            return true;
        else
            return false;
    }
    
    public void PlatformCheck()
    {
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down*(hitbox.size.y+0.5f), new Color(0,1,0));
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down,hitbox.size.y+0.5f,groundLayer);
        if(raycast.collider == null){
            nextMove= nextMove*(-1); 
            CancelInvoke(); //think를 잠시 멈춘 후 재실행
            Invoke("Think",2); 
        }
    }
    public void WallCheck()
    {
        Vector2 frontVec = hitbox.bounds.center;
        Debug.DrawRay(frontVec,new Vector2(nextMove, 0) * hitbox.size , new Color(0,1,0));
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, new Vector2(nextMove, 0) ,hitbox.size.x,groundLayer);
        if(raycast.collider != null){
            nextMove= nextMove*(-1); 
            CancelInvoke();
        }
    }
}
