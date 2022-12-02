using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InventoryUI : MonoBehaviour
{
    private List<ItemSlotUI> _slotUIList = new List<ItemSlotUI>(20);
    private InventoryController _inventory;
    //슬롯 초기화
    public void InitSlots()
    {
        for(int i=0;i<20;i++)
        {
            var slotUI = transform.GetChild(i).gameObject.GetComponent<ItemSlotUI>();
            slotUI.SetSlotIndex(i);
            _slotUIList.Add(slotUI);
        }
    }

    public void SetInventoryReference(InventoryController inventory)
    {
        _inventory = inventory;
    }

    // 슬롯에 아이템 아이콘 등록
    public void SetItemIcon(int index, Sprite icon)
    {
        _slotUIList[index].SetItem(icon);
    }
    // 해당 슬롯의 아이템 개수 텍스트 지정
    public void SetItemAmountText(int index, int amount)
    {
        // NOTE : amount가 1 이하일 경우 텍스트 미표시
        _slotUIList[index].SetItemAmount(amount);
    }
    // 해당 슬롯의 아이템 개수 텍스트 지정
    public void HideItemAmountText(int index)
    {
        _slotUIList[index].SetItemAmount(1);
    }
    // 슬롯에서 아이템 아이콘 제거, 개수 텍스트 숨기기
    public void RemoveItem(int index)
    {
        _slotUIList[index].RemoveItem();
    }
}
