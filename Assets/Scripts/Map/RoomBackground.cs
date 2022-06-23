using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBackground : MonoBehaviour
{
    Room room;
    
    private void Awake() {
        room = transform.parent.gameObject.GetComponent<Room>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player")
        {
            room.controller.SetNextRoom(this.room);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player")
        {
            room.controller.SetCurrentRoom();
            room.HideAll();
        }
    }
}
