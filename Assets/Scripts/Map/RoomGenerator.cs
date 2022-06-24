using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{

    //방은 maxTile의 반을 버림한 값이 기준
    //예시로 maxTile=11이면 기준은 5,5

    List<RoomInfo> roomList;
    List<RoomInfo> endRoomList;

    [SerializeField] int maxRoom;
    [SerializeField] GameObject testMap;
    [SerializeField] RoomTemplate template;
    
    GameObject temp;

    const int maxTile = 11;
    bool[,] isCreated;

    int roomCount;
    int makeRoomCount;

    private void Awake() {
        isCreated = new bool[maxTile,maxTile];
        roomList = new List<RoomInfo>();
        endRoomList = new List<RoomInfo>();
        roomCount = 0;
    }

    public void RoomInit()
    {
        makeRoomCount = Random.Range(maxRoom-4, maxRoom);
        while(roomCount < makeRoomCount)
        {
            roomCount = 0;
            for(int i=0;i<maxTile;i++)
                for(int j=0;j<maxTile;j++)
                    isCreated[i,j]=false;
            roomList.Clear();
            endRoomList.Clear();
            CreateDungeon();
        }
        if(endRoomList.Count <= 1)
        {
            CreateEndRoom();
        }
        SetPath();
        endRoomList.Sort((r1, r2) => r2.distance.CompareTo(r1.distance));
        InputRoomTemplate();
    }
    
    public bool CreateRoom(Vector2Int position, RoomType type, int distance)
    {
        bool isNoChildRoom = true;
        if(roomCount >= makeRoomCount)
            return false;
        if(position.x < 0 || position.x >= maxTile)
            return false;
        if(position.y < 0 || position.y >= maxTile)
            return false;
        if(isCreated[position.y,position.x]==true)
            return false;
        if(isNearTwo(position.y,position.x))
            return false;
        if(Random.Range(0,2)==0)
            return false;
        
        Vector2Int roomPos = new Vector2Int(position.x, position.y);
        RoomInfo room = new RoomInfo();
        distance++;
        room.SetRoom(roomPos, RoomType.Normal, distance);
        roomList.Add(room);
        isCreated[position.y,position.x] = true;
        roomCount++;

        if(CreateRoom(roomPos+Vector2Int.left, RoomType.Normal, distance))
            isNoChildRoom = false;
        if(CreateRoom(roomPos+Vector2Int.up, RoomType.Normal, distance))
            isNoChildRoom = false;
        if(CreateRoom(roomPos+Vector2Int.right, RoomType.Normal, distance))
            isNoChildRoom = false;
        if(CreateRoom(roomPos+Vector2Int.down, RoomType.Normal, distance))
            isNoChildRoom = false;
        if(isNoChildRoom)
        {
            endRoomList.Add(room);
        }
        return true;
    }

    public void CreateDungeon()
    {
        Vector2Int roomPos = new Vector2Int(maxTile/2, maxTile/2);
        RoomInfo room = new RoomInfo();
        int distance = 0;
        room.SetRoom(roomPos, RoomType.Start, distance);
        roomList.Add(room);
        isCreated[maxTile/2,maxTile/2] = true;

        CreateRoom(roomPos+Vector2Int.left, RoomType.Normal, distance);
        CreateRoom(roomPos+Vector2Int.up, RoomType.Normal, distance);
        CreateRoom(roomPos+Vector2Int.right, RoomType.Normal, distance);
        CreateRoom(roomPos+Vector2Int.down, RoomType.Normal, distance);
    }
    bool isNearTwo(int y, int x)
    {
        int nearCount = 0;
        if(isCreated[Mathf.Clamp(y-1,0,maxTile-1),x]==true)
            nearCount++;
        if(isCreated[Mathf.Clamp(y+1,0,maxTile-1),x]==true)
            nearCount++;
        if(isCreated[y,Mathf.Clamp(x-1,0,maxTile-1)]==true)
            nearCount++;
        if(isCreated[y,Mathf.Clamp(x+1,0,maxTile-1)]==true)
            nearCount++;
        if(nearCount>=2)
            return true;
        else
            return false;
    }

    void CreateEndRoom()
    {
        bool isCreated = false;
        RoomInfo pivotRoom = roomList[Random.Range(0,roomList.Count-1)];
        roomCount--;
        while(!isCreated)
        {
            CreateRoom(pivotRoom.pos+Vector2Int.left, RoomType.Normal, pivotRoom.distance);
            CreateRoom(pivotRoom.pos+Vector2Int.up, RoomType.Normal, pivotRoom.distance);
            CreateRoom(pivotRoom.pos+Vector2Int.right, RoomType.Normal, pivotRoom.distance);
            CreateRoom(pivotRoom.pos+Vector2Int.down, RoomType.Normal, pivotRoom.distance);
            if(roomCount == makeRoomCount)
                isCreated = true;
        }
        roomCount++;
    }

    void SetPath()
    {
        for(int i=0;i<roomList.Count;i++)
        {
            int x = roomList[i].pos.x;
            int y = roomList[i].pos.y;
            if(x!=0)
                if(isCreated[y,x-1]==true)
                    roomList[i].path[0] = true;
            if(y!=maxTile-1)
                if(isCreated[y+1,x]==true)
                    roomList[i].path[1] = true;
            if(x!=maxTile-1)
                if(isCreated[y,x+1]==true)
                    roomList[i].path[2] = true;
            if(y!=0)
                if(isCreated[y-1,x]==true)
                    roomList[i].path[3] = true;
        }
    }
    void InputRoomTemplate()
    {
        endRoomList[0].roomType = RoomType.Boss;
        endRoomList[1].roomType = RoomType.ItemShop;

        for(int i = 0;i<roomList.Count;i++)
        {
            if(roomList[i].roomType == RoomType.Boss)
            {
                roomList[i].roomPrefab = template.BossRoom[Random.Range(0,template.BossRoom.Length)];
            }
            else if(roomList[i].roomType == RoomType.Start)
            {
                roomList[i].roomPrefab = template.StartRoom[Random.Range(0,template.StartRoom.Length)];
            }
            else if(roomList[i].roomType == RoomType.ItemShop)
            {
                roomList[i].roomPrefab = template.ItemRoom[Random.Range(0,template.ItemRoom.Length)];
            }
            else
            {
                roomList[i].roomPrefab = template.NormalRoom[Random.Range(0,template.NormalRoom.Length)];
            }
        }
    }
    public List<RoomInfo> GetRoomList()
    {
        return roomList;
    }
    public void NextStageCreate()
    {
        roomList.Clear();
        endRoomList.Clear();
        roomCount=0;
        RoomInit();
    }
}
