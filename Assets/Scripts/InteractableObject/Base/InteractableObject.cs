using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    bool isOnPlayer = false;
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
            isOnPlayer = true;
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player")
            isOnPlayer = false;
    }

    protected abstract void Interaction();
}
