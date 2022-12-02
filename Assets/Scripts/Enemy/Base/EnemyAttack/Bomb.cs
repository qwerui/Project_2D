using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour //보스의 대포 공격
{
    int damage;
    [SerializeField] Animator ani;
    [SerializeField] CapsuleCollider2D hitbox;
    [SerializeField] Rigidbody2D rigid;
    
    public void SetBomb(int dmg, Vector2 force)
    {
        damage = dmg;
        rigid.AddForce(force, ForceMode2D.Impulse);
    }
    //낙하시 각도 변환
    private void FixedUpdate() {
        if(rigid.velocity.y >=0)
            rigid.rotation = -Vector2.Angle(Vector2.left, rigid.velocity);
        else
            rigid.rotation = Vector2.Angle(Vector2.left, rigid.velocity);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player") //플레이어 충돌
        {
            ani.SetTrigger("Explosion");
            hitbox.enabled = false;
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            other.gameObject.GetComponent<PlayerController>().Damaged(this.transform.position, damage);
            Invoke("DestroyThis", 0.5f);
        }
        else if(other.gameObject.tag == "Ground") //지면 폭발에도 판정 존재
        {
            ani.SetTrigger("Explosion");
            hitbox.offset = Vector2.zero;
            hitbox.size = new Vector2(1,1);
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            Invoke("DestroyThis", 0.5f);
        }
    }
    void DestroyThis()
    {
        Destroy(gameObject);
    }
}
