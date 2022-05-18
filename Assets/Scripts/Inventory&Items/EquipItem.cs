using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EquipItem : Item
{
    public EquipItemData equipItemData { get; private set; }
    public UnityEvent itemEvent;

    public EquipItem(EquipItemData data, UnityEvent eventFunc = null) : base(data)
    {
        itemEvent = eventFunc;
        equipItemData = data;
    }

    public bool Equip()
    {
        itemEvent.Invoke();
        return true;
    }
}
