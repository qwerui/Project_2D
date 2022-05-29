using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    protected bool isOnPlayer = false;
    protected InteractionDirector interactionDirector;
    private void Update() {
        if(isOnPlayer)
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
