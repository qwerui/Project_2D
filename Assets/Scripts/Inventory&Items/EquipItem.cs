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


    //UnityEvent에 increase 메소드는 앞에 decrease 메소드는 뒤에 넣어야함
    public bool Equip()
    {
        equipItemData.DropItemPrefab.GetComponent<ItemPrefab>().ItemEffect(true);
        return true;
    }
    public bool UnEquip()
    {
        equipItemData.DropItemPrefab.GetComponent<ItemPrefab>().ItemEffect(false);
        return true;
    }
}
