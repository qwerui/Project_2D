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
        for(int i=0;i<transform.childCount;i++)
        {
            if(Random.Range(0,100) < existPercent[i])
            {
                if(transform.GetChild(i).gameObject.tag == "EquipDrop")
                {
                    Instantiate(EquipItemList[Random.Range(0,EquipItemList.Count)].DropItemPrefab, transform.GetChild(i));
                }
                else
                {
                    Instantiate(SubItemList[Random.Range(0,SubItemList.Count)].DropItemPrefab, transform.GetChild(i));
                }
            }
        }
    }
}
