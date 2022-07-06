using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    public int[] existPercent;
    private List<ItemData> SubItemList;
    private List<ItemData> EquipItemList;
    private void Awake() {
        EquipItemList = ItemLoader.Instance.GetEquipItemList();
        SubItemList = ItemLoader.Instance.GetSubItemList();
    }
    void Start()
    {
        if(DataDirector.Instance.isLoadedGame)
        {
            int index = transform.parent.parent.gameObject.GetComponent<Room>().roomIndex;
            string[] roomItem = GameObject.Find("DataController").GetComponent<DataController>().LoadRoomItem(index).Split(',');
            for(int i=0;i<transform.childCount;i++)
            {
                int itemIndex = int.Parse(roomItem[i]);
                if(itemIndex == 0)
                {
                    continue;
                }
                if (transform.GetChild(i).gameObject.tag == "EquipDrop")
                {
                    Instantiate(ItemLoader.Instance.GetEquipItem(itemIndex).DropItemPrefab, transform.GetChild(i));
                }
                else
                {
                    Instantiate(ItemLoader.Instance.GetSubItem(itemIndex).DropItemPrefab, transform.GetChild(i));
                }
            }
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (Random.Range(0, 100) < existPercent[i])
                {
                    if (transform.GetChild(i).gameObject.tag == "EquipDrop")
                    {
                        Instantiate(EquipItemList[Random.Range(0, EquipItemList.Count)].DropItemPrefab, transform.GetChild(i));
                    }
                    else
                    {
                        Instantiate(SubItemList[Random.Range(0, SubItemList.Count)].DropItemPrefab, transform.GetChild(i));
                    }
                }
            }
        }  
    }
}
