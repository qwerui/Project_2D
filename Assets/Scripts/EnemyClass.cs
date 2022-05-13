using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyClass : MonoBehaviour
{
    [SerializeField] protected int hp;
    [SerializeField] protected int atk;
    [SerializeField] protected int def;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Animator ani;
    [SerializeField] protected Rigidbody2D rigid;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected BoxCollider2D hitbox;
    [SerializeField] protected GameObject player;

    protected int nextMove;
    protected float posDiff;

    protected bool isMoving;

    protected abstract void EnemyInit();
    protected abstract void Move(); //적 이동
    protected abstract void Attack(); //적 공격
    protected abstract void Think(); //적 이동 AI
    protected abstract void Chase(); //적 추적
    protected abstract IEnumerator PosDiff(); //적 추적시 방향 전환 속도 조절 (적과 캐릭터의 좌표 차이)

    protected void Hit()
    {
        ani.SetTrigger("Hit");
    }
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        
        if(collision.gameObject.tag == "Player"){
            player.GetComponent<PlayerContoller>().Damaged(this.transform.position, this.atk);
        }

    }
    protected bool SetOnChase(float radius)
    {
        if(Physics2D.OverlapCircle((Vector2)transform.position + hitbox.offset,radius,LayerMask.GetMask("Player")))
            return true;
        else
            return false;
    }
    
    protected void PlatformCheck()
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
    protected void WallCheck()
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
