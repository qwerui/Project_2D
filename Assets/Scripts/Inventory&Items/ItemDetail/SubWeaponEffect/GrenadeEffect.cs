using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeEffect : MonoBehaviour
{
    //수류탄 효과
    Rigidbody2D rigid;
    int damage;
    int direction;
    public Animator ani;
    AudioSource sound;
    private void Start() {
        rigid = GetComponent<Rigidbody2D>();
        rigid.AddForce(new Vector2(3*direction,15), ForceMode2D.Impulse);//투척 효과
        sound = GetComponent<AudioSource>();
    }
    public void SetEffect(GameObject player, PlayerStatus stat)
    {
        transform.position = player.transform.position + Vector3.up * 1.75f;
        transform.localScale = player.transform.localScale;
        direction = (int)transform.localScale.x;
        damage = stat.getAtk()*5;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Ground")//지면 충돌시 폭발
        {
            Explosion();
        }
        if(other.gameObject.tag == "Enemy")//적 충돌시 폭발
        {
            other.gameObject.GetComponent<EnemyClass>().Hit(damage);
            Explosion();
        }
    }
    void Explosion() //폭발 효과
    {
        sound.PlayOneShot(sound.clip);
        ani.SetTrigger("explosion");
        rigid.bodyType = RigidbodyType2D.Static;
        Destroy(gameObject, 0.5f);
    }
}
