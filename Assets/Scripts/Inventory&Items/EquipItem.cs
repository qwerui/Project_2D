using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipItem : Item
{
    public EquipItemData equipItemData { get; private set; }

    public EquipItem(EquipItemData data) : base(data)
    {
        equipItemData = data;
    }

    public bool Equip()
    {
        equipItemData.DropItemPrefab.GetComponent<ItemPrefab>().ItemEffect(this, true);
        return true;
    }
    public bool UnEquip()
    {
        equipItemData.DropItemPrefab.GetComponent<ItemPrefab>().ItemEffect(this, false);
        return true;
    }
}
