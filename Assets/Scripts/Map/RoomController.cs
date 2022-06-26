using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    List<RoomInfo> roomList;

    [SerializeField] GameObject roomGenerator;
    [SerializeField] GameObject MapTile;
    [SerializeField] GameObject cameraObj;

    CameraController cameraCtl;
    RoomGenerator generator;
    Room currentRoom;
    Room nextRoom;

    int maxTile;

    private void Awake() {
        roomList = new List<RoomInfo>();
    }

    void Start()
    {
        generator = roomGenerator.GetComponent<RoomGenerator>();
        cameraCtl = cameraObj.GetComponent<CameraController>();
        ArrangeRooms();
    }

    void ArrangeRooms()
    {
        if(roomList.Count <=0)
        {
            generator.RoomInit();
            roomList = generator.GetRoomList();
        }
        
        maxTile = generator.GetMaxTile();
        for(int i=0;i<roomList.Count;i++)
        {
            GameObject tempMap = Instantiate(roomList[i].roomPrefab, MapTile.transform) as GameObject;
            tempMap.transform.position = new Vector2(roomList[i].pos.x-(int)(maxTile/2),roomList[i].pos.y-(int)(maxTile/2))*50;
            tempMap.GetComponent<Room>().SetRoomInfo(roomList[i], this);
        }
    }
    public void SetCurrentRoom()
    {
        currentRoom = nextRoom;
        currentRoom.RoomInit();
        cameraCtl.SetCameraLimit(currentRoom.roomInfo, maxTile);
    }
    public void SetNextRoom(Room next)
    {
        nextRoom = next;
    }
    public void NextStage()
    {
        for(int i=0;i<MapTile.transform.childCount;i++)
        {
            Destroy(MapTile.transform.GetChild(i).gameObject);
        }
        roomList.Clear();
        generator.NextStageCreate();
        ArrangeRooms();
        cameraCtl.SetCameraLimit(roomList[0], maxTile);
    }
    public List<RoomInfo> GetRoomList()
    {
        return this.roomList;
    }
}
