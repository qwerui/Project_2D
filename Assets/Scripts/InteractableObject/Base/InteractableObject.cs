using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    //상호작용 오브젝트 클래스
    protected bool isOnPlayer = false;
    protected InteractionDirector interactionDirector;
    private void Update() {
        if(isOnPlayer) //플레이어와 겹칠 시 상호작용 가능
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Interaction();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            if(interactionDirector == null)
                interactionDirector = other.gameObject.transform.GetChild(1).GetChild(0).gameObject.GetComponent<InteractionDirector>();
            isOnPlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            ExitAction();
            isOnPlayer = false;
        }
    }

    protected abstract void Interaction();
    protected abstract void ExitAction();
}
