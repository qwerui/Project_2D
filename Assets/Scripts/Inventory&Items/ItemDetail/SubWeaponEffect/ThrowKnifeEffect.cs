using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowKnifeEffect : MonoBehaviour
{
    Rigidbody2D rigid;
    int damage;
    int direction;
    float delta;
    private void Start() {
        rigid = GetComponent<Rigidbody2D>();
    }
    public void SetEffect(GameObject player, PlayerStatus stat)
    {
        transform.position = player.transform.position + Vector3.up * 1.75f;
        transform.localScale = player.transform.localScale;
        direction = (int)transform.localScale.x;
        damage = stat.getAtk()/5;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Enemy")
        {
            other.GetComponent<EnemyClass>().Hit(damage);
            Destroy(gameObject);
        }
    }
    private void Update() {
        delta+=Time.deltaTime;
        if(delta >= 5.0f)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate() {
        rigid.velocity = new Vector2(20*direction,0);
    }
}
