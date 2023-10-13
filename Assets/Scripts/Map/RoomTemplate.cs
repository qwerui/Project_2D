using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomTemplate", menuName = "Project_2D/RoomTemplate", order = 6)]
public class RoomTemplate : ScriptableObject {
    //방 템플릿 그룹화
    public GameObject[] NormalRoom;
    public GameObject[] BossRoom;
    public GameObject[] StartRoom;
    public GameObject[] ItemRoom;
}
