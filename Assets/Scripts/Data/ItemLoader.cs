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
        if(SubItemList == null || EquipItemList == null)
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
    public ItemData GetEquipItem(int id)
    {
        return EquipItemList.Find(x => x.ID == id);
    }
    public ItemData GetSubItem(int id)
    {
        return SubItemList.Find(x => x.ID == id);
    }
    public int GetSubItemCount()
    {
        return SubItemList.Count;
    }
    public int GetEquipItemCount()
    {
        return EquipItemList.Count;
    }
}
