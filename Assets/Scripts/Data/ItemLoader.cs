using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLoader
{
    private static ItemLoader instance = null;
    
    private static List<ItemData> SubItemList;
    private static List<ItemData> EquipItemList;

    public static ItemLoader Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new ItemLoader();
            }
            return instance;
        }
    }
    public ItemLoader()
    {
        InitItemList();
    }
    private void InitItemList()
    {
        SubItemList = new List<ItemData>(Resources.LoadAll<ItemData>("ItemData/SubItem"));
        EquipItemList = new List<ItemData>(Resources.LoadAll<ItemData>("ItemData/EquipItem"));
    }
    public List<ItemData> GetEquipItemList()
    {
        return EquipItemList;
    }
    public List<ItemData> GetSubItemList()
    {
        return SubItemList;
    }
}
