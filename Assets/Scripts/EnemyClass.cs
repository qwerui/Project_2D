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

    protected bool isMoving;

    public abstract void Move();
    public abstract void Attack();
    public void Hit()
    {
        ani.SetTrigger("Hit");
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        
        if(collision.gameObject.tag == "Player"){
            GameObject.Find("Player").GetComponent<PlayerContoller>().Damaged(this.transform.position);
        }

    }
}
