using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] BoxCollider2D hitBox;
    int damage;
    void Start()
    {
        damage = 10;
    }

    public void ShootBeam()
    {
        transform.localScale = new Vector3(-20,1,1);
        hitBox.size = new Vector2(0.75f, 0.785f);
        StartCoroutine("Beam");
    }

    IEnumerator Beam()
    {
        while(true)
        {
            transform.localScale=transform.localScale + Vector3.down * 0.1f;
            hitBox.size = hitBox.size + Vector2.down * 0.0785f;
            yield return new WaitForSeconds(0.1f);
            if(transform.localScale.y <= 0)
                break;
        }
        gameObject.SetActive(false);
        yield return null;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag=="Player")
        {
            other.gameObject.GetComponent<PlayerController>().Damaged(transform.parent.parent.position, damage);
        }
    }
}
