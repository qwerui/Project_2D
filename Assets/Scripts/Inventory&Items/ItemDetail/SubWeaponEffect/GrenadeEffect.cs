using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeEffect : MonoBehaviour
{
    Rigidbody2D rigid;
    int damage;
    int direction;
    public Animator ani;
    AudioSource sound;
    private void Start() {
        rigid = GetComponent<Rigidbody2D>();
        rigid.AddForce(new Vector2(3,15), ForceMode2D.Impulse);
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
        if(other.gameObject.tag == "Ground")
        {
            Explosion();
        }
        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyClass>().Hit(damage);
            Explosion();
        }
    }
    void Explosion()
    {
        sound.PlayOneShot(sound.clip);
        ani.SetTrigger("explosion");
        rigid.bodyType = RigidbodyType2D.Static;
        Destroy(gameObject, 0.5f);
    }
}
