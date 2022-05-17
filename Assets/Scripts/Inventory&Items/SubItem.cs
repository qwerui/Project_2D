using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SubItem : Item
{
    public SubItemData subItemData { get; private set; }
    public UnityEvent itemEvent;

    // 현재 아이템 개수
    public int Amount { get; protected set; }

    // 개수가 없는지 여부
    public bool IsEmpty => Amount <= 0;


    public SubItem(SubItemData data, UnityEvent eventFunc = null, int amount = 1) : base(data)
    {
        itemEvent = eventFunc;
        subItemData = data;
        SetAmount(amount);
    }

    // 개수 설정
    public void SetAmount(int amount)
    {
        if(amount<0)
            Amount = 0;
        else
            Amount=amount;
    }
    public void AddAmount(int amount)
    {
        Amount += amount;
    }

    // 보조 아이템 사용
    public bool Use()
    {
        Amount--;
        itemEvent.Invoke();
        return true;
    }
}
