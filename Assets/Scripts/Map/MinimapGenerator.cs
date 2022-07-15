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
    public void SetMinimap(List<RoomInfo> roomList, int maxTile)
    {
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
            tempMinimap.SetActive(false);
        }
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public void ShowMinimap(int index)
    {
        GameObject tempMinimap = transform.GetChild(index).gameObject;
        tempMinimap.SetActive(true);
        tempMinimap.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        rect.anchoredPosition = new Vector2(-(roomList[index].pos.x - (maxTile / 2)), -(roomList[index].pos.y - (maxTile / 2))) * 16;
    }
    public void MinimapDark(int index)
    {
        transform.GetChild(index).gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    }
}
