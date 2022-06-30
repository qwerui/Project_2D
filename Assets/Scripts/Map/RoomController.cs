using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    List<RoomInfo> roomList;

    [SerializeField] GameObject roomGenerator;
    [SerializeField] GameObject MapTile;
    [SerializeField] GameObject cameraObj;
    [SerializeField] GameObject Minimap;
    [SerializeField] GameObject DataCtl;

    CameraController cameraCtl;
    RoomGenerator generator;
    MinimapGenerator minimapCtl;
    Room currentRoom;
    Room nextRoom;
    DataController data;

    int maxTile;
    int currentRoomIndex;

    bool isLoaded;

    private void Awake() {
        roomList = new List<RoomInfo>();
    }

    void Start()
    {
        isLoaded = DataDirector.Instance.isLoadedGame;
        generator = roomGenerator.GetComponent<RoomGenerator>();
        cameraCtl = cameraObj.GetComponent<CameraController>();
        minimapCtl = Minimap.GetComponent<MinimapGenerator>();
        if(isLoaded)
        {
            data = DataCtl.GetComponent<DataController>();
            LoadRoomList();
        }
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
        minimapCtl.SetMinimap(roomList, maxTile);
        if(isLoaded)
        {
            for(int i=0;i<roomList.Count;i++)
            {
                if(roomList[i].isVisited == true)
                {
                    minimapCtl.ShowMinimap(i);
                    minimapCtl.MinimapDark(i);
                }
            }
            currentRoomIndex = DataDirector.Instance.playerPosIndex;
            minimapCtl.ShowMinimap(currentRoomIndex);
            cameraCtl.SetCameraLimit(roomList[currentRoomIndex], maxTile);
            cameraCtl.SetCameraPosition();
        }
        else
        {
            currentRoomIndex = 0;
            DataDirector.Instance.playerPosIndex = 0;
        }
    }
    public void SetCurrentRoom()
    {
        minimapCtl.MinimapDark(currentRoomIndex);
        currentRoom = nextRoom;
        currentRoom.RoomInit();
        currentRoomIndex = roomList.IndexOf(currentRoom.roomInfo);
        minimapCtl.ShowMinimap(currentRoomIndex);
        currentRoom.roomInfo.isVisited = true;
        cameraCtl.SetCameraLimit(currentRoom.roomInfo, maxTile);
        DataDirector.Instance.playerPosIndex = currentRoomIndex;
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
    void LoadRoomList()
    {
        for(int i=0;i<data.GetLoadedRoomCount();i++)
        {
            roomList.Add(generator.LoadRoomPrefab(data.LoadRoomInfo(i)));
        }
        generator.SetLoadedRoomPath(roomList);
    }
}
