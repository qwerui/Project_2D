using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HalfFloor : MonoBehaviour //반칸 블록
{
    PlatformEffector2D effector;
    const int exceptPlayer = 1073774007; //플레이어 제외 충돌
    const int includePlayer = 2147483647; //플레이어 포함 충돌
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
            if(Input.GetKey(KeyCode.DownArrow)&&Input.GetKeyDown(KeyCode.X)) //아래키+점프로 아래로 내려갈 수 있음
            {
                effector.colliderMask = exceptPlayer;
                Invoke("RestoreEffector", 0.3f);
                break;
            }
            yield return null;
        }
    }
    //충돌 상태 복원
    void RestoreEffector()
    {
        effector.colliderMask = includePlayer;
        StopAllCoroutines();
    }
}
