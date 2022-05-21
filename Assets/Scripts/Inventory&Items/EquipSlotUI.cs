using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlotUI : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private EquipItem item;
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
        item = inventoryItem as EquipItem;
        _iconImage.sprite = inventoryItem.Data.IconSprite;
        item.Equip();
        ShowIcon();
        return false;
    }
    public void RemoveItem()
    {
        _iconImage.sprite = null;
        item.UnEquip();
        item = null;
        HideIcon();
    }
    public Item SwapItem(Item newItem)
    {
        Item tempItem = item;
        item = newItem as EquipItem;
        _iconImage.sprite = newItem.Data.IconSprite;
        return tempItem;
    }
    public EquipItem GetItem()
    {
<<<<<<< Updated upstream
        return item as EquipItem;
=======
        if(item != null)
        {
            Instantiate(item.equipItemData.WeaponEffect);
        }
>>>>>>> Stashed changes
    }
}
