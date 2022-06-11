using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public RoomInfo roomInfo;
    RoomController controller;
    // Start is called before the first frame update
    void Start()
    {
        if(roomInfo.path[0] != true)
        {
            transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        }
        if(roomInfo.path[1] != true)
        {
            transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
        }
        if(roomInfo.path[2] != true)
        {
            transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
        }
        if(roomInfo.path[3] != true)
        {
            transform.GetChild(1).GetChild(3).gameObject.SetActive(true);
        }
    }
    public void SetRoomInfo(RoomInfo info, RoomController ctl)
    {
        roomInfo = info;
        roomInfo.Width = 50;
        roomInfo.Height = 50;
        controller = ctl;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player")
        {
            controller.SetCurrentRoom(this);
        }
    }
}
