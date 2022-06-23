using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomInfo
{
    public int Width;
    public int Height;
    public int distance;

    public RoomType roomType;
    public GameObject roomPrefab;

    public Vector2Int pos;
    public bool[] path; //0: 왼쪽, 1:위쪽, 2:오른쪽, 3:아래

    public void SetRoom(Vector2Int position, RoomType type, int distance)
    {
        this.roomType = type;
        this.pos = position;
        this.distance = distance;
        this.path = new bool[4];
    }
    
}
public enum RoomType
{
    Start,
    Normal,
    ItemShop,
    Boss
}