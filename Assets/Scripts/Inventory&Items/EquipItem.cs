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


    //UnityEvent에 increase 메소드는 앞에 decrease 메소드는 뒤에 넣어야함
    public bool Equip()
    {
        for(int i=0;i<itemEvent.GetPersistentEventCount()/2;i++)
        {
            itemEvent.SetPersistentListenerState(i, UnityEventCallState.RuntimeOnly);
            itemEvent.SetPersistentListenerState(itemEvent.GetPersistentEventCount()-(i+1), UnityEventCallState.Off);
        }
        itemEvent.Invoke();
        return true;
    }
    public bool UnEquip()
    {
        for(int i=0;i<itemEvent.GetPersistentEventCount()/2;i++)
        {
            itemEvent.SetPersistentListenerState(i, UnityEventCallState.Off);
            itemEvent.SetPersistentListenerState(itemEvent.GetPersistentEventCount()-(i+1), UnityEventCallState.RuntimeOnly);
        }
        itemEvent.Invoke();
        return true;
    }
}
