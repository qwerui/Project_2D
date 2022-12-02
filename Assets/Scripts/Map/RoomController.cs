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
    //스테이지 생성
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
            tempMap.GetComponent<Room>().SetRoomInfo(roomList[i], this, i);
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
    //현재 방 설정
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
    //다음 방 설정
    public void SetNextRoom(Room next)
    {
        nextRoom = next;
    }
    //다음 스테이지 진행
    public void NextStage(SoundDirector sound)
    {
        StartCoroutine(NextStageSound(sound));
        for(int i=0;i<MapTile.transform.childCount;i++)
        {
            Destroy(MapTile.transform.GetChild(i).gameObject);
        }
        roomList.Clear();
        generator.NextStageCreate();
        ArrangeRooms();
        minimapCtl.ShowMinimap(0);
        cameraCtl.SetCameraLimit(roomList[0], maxTile);
        cameraCtl.SetCameraPosition();

    }
    public List<RoomInfo> GetRoomList()
    {
        return this.roomList;
    }
    public RoomInfo GetRoomInfo(int index)
    {
        return this.roomList[index];
    }
    //맵 불러오기
    void LoadRoomList()
    {
        for(int i=0;i<data.GetLoadedRoomCount();i++)
        {
            roomList.Add(generator.LoadRoomPrefab(data.LoadRoomInfo(i)));
        }
        generator.SetLoadedRoomPath(roomList);
    }
    public string GetRoomItemList(int index)
    {
        return MapTile.transform.GetChild(index).gameObject.GetComponent<Room>().GetRoomItemId();
    }
    //다음 스테이지 소리 출력
    IEnumerator NextStageSound(SoundDirector sound)
    {
        for (int i = 0; i < 4; i++)
        {
            sound.SetFxPitch(0.8f - (i * 0.2f));
            sound.SetFxVolume(0.6f - (i * 0.1f));
            sound.FxPlay(2);
            yield return new WaitForSeconds(0.25f);
        }
        sound.SetFxPitch(1);
        sound.SetFxVolume(1);
    }
}
