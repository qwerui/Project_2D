using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlotUI : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private Item item;
    [SerializeField] private ItemType type;
    [SerializeField] private GameObject player;

    private InventoryPopupUI _inventoryPopupUI;
    private GameObject _iconGo;
    
    private void ShowIcon() => _iconGo.SetActive(true);
    private void HideIcon() => _iconGo.SetActive(false);

    private void Awake() {
        _inventoryPopupUI = GameObject.Find("ItemPopup").GetComponent<InventoryPopupUI>();
        _iconGo = transform.GetChild(0).gameObject;
        HideIcon();
    }

    public void OpenItemPopup()
    {
        _inventoryPopupUI.ShowPanel();
        _inventoryPopupUI.ViewEquipItem(item, type);
    }
    public bool SetItem(Item inventoryItem)
    {
        if(item != null)
            return true;
        item = inventoryItem;
        _iconImage.sprite = inventoryItem.Data.IconSprite;
        ShowIcon();
        return false;
    }
    public void RemoveItem()
    {
        _iconImage.sprite = null;
        item = null;
        HideIcon();
    }
    public Item SwapItem(Item newItem)
    {
        Item tempItem = item;
        item = newItem;
        _iconImage.sprite = newItem.Data.IconSprite;
        return tempItem;
    }
}
