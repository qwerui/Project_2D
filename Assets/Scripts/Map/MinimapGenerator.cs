using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapGenerator : MonoBehaviour
{
    [SerializeField] GameObject minimapPrefab;
    RectTransform rect;
    List<RoomInfo> roomList;

    int maxTile;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }
    //미니맵 생성
    public void SetMinimap(List<RoomInfo> roomList, int maxTile)
    {
        if(transform.childCount!=0)
        {
            for(int i=0;i<transform.childCount;i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        this.roomList = roomList;
        this.maxTile = maxTile;
        for (int i = 0; i < roomList.Count; i++)
        {
            GameObject tempMinimap = Instantiate(minimapPrefab, transform) as GameObject;
            tempMinimap.GetComponent<RectTransform>().anchoredPosition = new Vector2(roomList[i].pos.x - (maxTile / 2), roomList[i].pos.y - (maxTile / 2)) * 16;
            for(int j=0;j<4;j++)
            {
                if(roomList[i].path[j] == true)
                {
                    tempMinimap.transform.GetChild(j).gameObject.SetActive(true);
                }
            }
            if(i!=0)
                tempMinimap.SetActive(false);
        }
    }
    //미니맵 출력
    public void ShowMinimap(int index)
    {
        GameObject tempMinimap = transform.GetChild(index).gameObject;
        tempMinimap.SetActive(true);
        tempMinimap.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        rect.anchoredPosition = new Vector2(-(roomList[index].pos.x - (maxTile / 2)), -(roomList[index].pos.y - (maxTile / 2))) * 16;
    }
    //현재 위치를 제외한 미니맵 어둡게 만듬
    public void MinimapDark(int index)
    {
        transform.GetChild(index).gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    }
}
