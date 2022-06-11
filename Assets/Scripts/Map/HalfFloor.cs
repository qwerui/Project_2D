using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HalfFloor : MonoBehaviour
{
    TilemapCollider2D tileCollider;
    private void Start() {
        tileCollider = GetComponent<TilemapCollider2D>();
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player")
        {
            StartCoroutine("DownFloor");
        }
    }
    IEnumerator DownFloor()
    {
        while(true)
        {
            if(Input.GetKey(KeyCode.DownArrow)&&Input.GetKeyDown(KeyCode.X))
            {
                tileCollider.enabled = false;
                Invoke("ColliderOn", 1f);
                break;
            }
            yield return null;
        }
    }
    void ColliderOn()
    {
        tileCollider.enabled = true;
        StopAllCoroutines();
    }
}
