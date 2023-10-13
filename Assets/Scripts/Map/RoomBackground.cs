using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBackground : MonoBehaviour
{
    Room room;
    //플레이어 현재 위치 추적
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
            room.roomInfo.isVisited = true;
            room.HideAll();
        }
    }
}
