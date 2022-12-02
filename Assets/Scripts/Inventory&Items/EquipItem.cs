using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipItem : Item
{
    //장비 아이템 객체
    public EquipItemData equipItemData { get; private set; }

    public EquipItem(EquipItemData data) : base(data)
    {
        equipItemData = data;
    }

    public bool Equip()//장비
    {
        equipItemData.DropItemPrefab.GetComponent<ItemPrefab>().ItemEffect(this, true);
        return true;
    }
    public bool UnEquip()//장비 해제
    {
        equipItemData.DropItemPrefab.GetComponent<ItemPrefab>().ItemEffect(this, false);
        return true;
    }
}
