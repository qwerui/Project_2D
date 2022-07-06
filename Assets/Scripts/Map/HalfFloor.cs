using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HalfFloor : MonoBehaviour
{
    PlatformEffector2D effector;
    const int exceptPlayer = 1073774007;
    const int includePlayer = 2147483647;
    private void Start() {
        effector = GetComponent<PlatformEffector2D>();
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
                effector.colliderMask = exceptPlayer;
                Invoke("RestoreEffector", 0.3f);
                break;
            }
            yield return null;
        }
    }
    void RestoreEffector()
    {
        effector.colliderMask = includePlayer;
        StopAllCoroutines();
    }
}
