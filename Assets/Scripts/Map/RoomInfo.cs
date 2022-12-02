using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo
{
    //방 정보
    public int roomId;//방 인덱스

    public int Width;
    public int Height;
    public int distance; //중앙 방에서 부터의 거리

    public RoomType roomType; //방 타입
    public GameObject roomPrefab; //방 프리팹

    public Vector2Int pos;//방 위치
    public bool[] path; //0: 왼쪽, 1:위쪽, 2:오른쪽, 3:아래 통로

    public bool isVisited;//방문 여부

    public void SetRoom(Vector2Int position, RoomType type, int distance)
    {
        this.roomType = type;
        this.pos = position;
        this.distance = distance;
        this.path = new bool[4];
        isVisited = false;
    }
}
public enum RoomType
{
    Start,
    Normal,
    ItemShop,
    Boss
}