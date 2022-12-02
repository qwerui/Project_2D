using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomGenerator : MonoBehaviour
{

    //방은 maxTile의 반을 버림한 값이 기준
    //예시로 maxTile=11이면 기준은 5,5

    List<RoomInfo> roomList;
    List<RoomInfo> endRoomList;

    int maxRoom;
    [SerializeField] GameObject testMap;
    [SerializeField] RoomTemplate template;

    const int maxTile = 15;
    bool[,] isCreated;

    int roomCount;
    int makeRoomCount;

    private void Awake() {
        isCreated = new bool[maxTile,maxTile];
        roomList = new List<RoomInfo>();
        endRoomList = new List<RoomInfo>();
        roomCount = 0;
    }
    //맵 초기화
    public void RoomInit()
    {
        int loopNum = 0;
        maxRoom = DataDirector.Instance.stage / 3 + 6;
        if (maxRoom >= 70)
            maxRoom = 70;
        makeRoomCount = Random.Range(maxRoom-2, maxRoom);
        while(roomCount < makeRoomCount) //생성 방 개수에 도달할 때 까지 루프
        {
            roomCount = 0;
            for(int i=0;i<maxTile;i++)
                for(int j=0;j<maxTile;j++)
                    isCreated[i,j]=false;
            roomList.Clear();
            endRoomList.Clear();
            CreateDungeon();
            if(loopNum++ > 1000)
                throw new System.Exception("RoomInit Loop");
        }
        if(endRoomList.Count <= 1)
        {
            CreateEndRoom();
        }
        SetPath();
        endRoomList.Sort((r1, r2) => r2.distance.CompareTo(r1.distance));
        InputRoomTemplate();
    }
    //방 생성
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
        //재귀
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
    //최초 중앙 방 생성
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
    //생성하려는 방 상하좌우로 방이 2개 이상 존재하면 방 생성 포기
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
    //막다른 방 개수가 모자를 경우 생성
    void CreateEndRoom()
    {
        int loopNum = 0;
        bool isCreated = false;
        roomCount--;
        while(!isCreated)
        {
            RoomInfo pivotRoom = roomList[Random.Range(0,roomList.Count-1)];
            CreateRoom(pivotRoom.pos+Vector2Int.left, RoomType.Normal, pivotRoom.distance);
            CreateRoom(pivotRoom.pos+Vector2Int.up, RoomType.Normal, pivotRoom.distance);
            CreateRoom(pivotRoom.pos+Vector2Int.right, RoomType.Normal, pivotRoom.distance);
            CreateRoom(pivotRoom.pos+Vector2Int.down, RoomType.Normal, pivotRoom.distance);
            if(roomCount >= makeRoomCount)
                isCreated = true;
            if(loopNum++ > 10000)
                throw new System.Exception("EndRoom Loop");
        }
        roomCount++;
    }
    //통로 설정
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
    //방 탬플릿 주입
    void InputRoomTemplate()
    {
        endRoomList[0].roomType = RoomType.Boss;
        for(int i=1;i<(int)(roomCount/10)+2;i++)
        {
            if(i>=endRoomList.Count)
                break;
            else
                endRoomList[i].roomType = RoomType.ItemShop;
        }
            

        for(int i = 0;i<roomList.Count;i++)
        {
            if(roomList[i].roomType == RoomType.Boss)
            {
                roomList[i].roomPrefab = template.BossRoom[roomList[i].roomId = Random.Range(0,template.BossRoom.Length)];
            }
            else if(roomList[i].roomType == RoomType.Start)
            {
                roomList[i].roomPrefab = template.StartRoom[roomList[i].roomId = Random.Range(0,template.StartRoom.Length)];
            }
            else if(roomList[i].roomType == RoomType.ItemShop)
            {
                roomList[i].roomPrefab = template.ItemRoom[roomList[i].roomId = Random.Range(0,template.ItemRoom.Length)];
            }
            else
            {
                roomList[i].roomPrefab = template.NormalRoom[roomList[i].roomId = Random.Range(0,template.NormalRoom.Length)];
            }
        }
    }
    public List<RoomInfo> GetRoomList()
    {
        return roomList;
    }
    public int GetMaxTile()
    {
        return maxTile;
    }
    //다음 스테이지 생성
    public void NextStageCreate()
    {
        roomList.Clear();
        endRoomList.Clear();
        roomCount=0;
        RoomInit();
    }
    //방 템플릿 불러오기
    public RoomInfo LoadRoomPrefab(RoomInfo info)
    {
        switch(info.roomType)
        {
            case RoomType.Start:
                info.roomPrefab = template.StartRoom[info.roomId];
                break;
            case RoomType.Boss:
                info.roomPrefab = template.BossRoom[info.roomId];
                break;
            case RoomType.ItemShop:
                info.roomPrefab = template.ItemRoom[info.roomId];
                break;
            default:
                info.roomPrefab = template.NormalRoom[info.roomId];
                break;
        }
        return info;
    }
    //방 통로 불러오기
    public void SetLoadedRoomPath(List<RoomInfo> infoList)
    {
        roomList = infoList;
        for(int i=0;i<roomList.Count;i++)
        {
            isCreated[roomList[i].pos.y, roomList[i].pos.x] = true;
        }
        SetPath();
    }
}
