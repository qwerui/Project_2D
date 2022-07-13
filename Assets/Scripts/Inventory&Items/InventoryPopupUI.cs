using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopupUI : MonoBehaviour
{
    Item item;
    ItemType itemType;
    InventoryController inventory;

    Image PopupImage;
    Text PopupText;
    Text PopupItemName;
    Text UseButtonText;
    GameObject SlotButton;
    GameObject DropButton;
    QuickSlotUI quickSlot;
    EquipSlotUI equipSlot;

    [SerializeField] GameObject _inventory;
    [SerializeField] GameObject player;
    [SerializeField] GameObject _quickSlotPotion;
    [SerializeField] GameObject _quickSlotWeapon;
    [SerializeField] GameObject weaponSlot;
    [SerializeField] GameObject armorSlot;
    [SerializeField] GameObject acessorySlot;

    int Index;

    public void ShowPanel() => gameObject.SetActive(true);
    public void HidePanel() => gameObject.SetActive(false);

    private void Awake() {
        inventory = _inventory.GetComponent<InventoryController>();
        PopupImage = transform.GetChild(0).gameObject.GetComponent<Image>();
        PopupText = transform.GetChild(1).gameObject.GetComponent<Text>();
        PopupItemName = transform.GetChild(2).gameObject.GetComponent<Text>();
        DropButton = transform.GetChild(3).gameObject;
        SlotButton = transform.GetChild(4).gameObject;
        UseButtonText = transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        GetComponent<RectTransform>().anchoredPosition = new Vector2(-transform.parent.parent.gameObject.GetComponent<RectTransform>().sizeDelta.x / 2, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        HidePanel();
    }

    public void SetPopupItem(int index)
    {
        Index = index;
        item = inventory.ReturnItem(index);
        PopupImage.sprite = item.Data.IconSprite;
        UseButtonText.fontSize = 20;
        PopupText.text = item.Data.Tooltip;
        DropButton.SetActive(true);
        if(item is SubItem)
        {
            PopupItemName.text = item.Data.Name+"  (개수 : "+(item as SubItem).Amount+" )";
            UseButtonText.text = "USE";
            SlotButton.SetActive(true);
        }
        else
        {
            PopupItemName.text = item.Data.Name;
            UseButtonText.text = "EQUIP";
            SlotButton.SetActive(false);
        }
    }

    public void UseItem()
    {
        if(Index == -1)
        {
            bool successfulAdd;
            if(itemType == ItemType.Weapon)
                equipSlot = weaponSlot.GetComponent<EquipSlotUI>();
            else if(itemType == ItemType.Armor)
                equipSlot = armorSlot.GetComponent<EquipSlotUI>();
            else if(itemType == ItemType.Accesory)
                equipSlot = acessorySlot.GetComponent<EquipSlotUI>();
            successfulAdd = inventory.AddUnequipItem(item);
            if(successfulAdd)
            {
                equipSlot.RemoveItem();
            }
            HidePanel();
            return;
        }
        if(item is SubItem)
        {
            (item as SubItem).Use();
        }
        else
        {
            bool equipping = false;
            if(item.Data.ItemType == ItemType.Weapon)
            {
                equipSlot = weaponSlot.GetComponent<EquipSlotUI>();

            }
            else if(item.Data.ItemType == ItemType.Armor)
            {
                equipSlot = armorSlot.GetComponent<EquipSlotUI>();
            }
            else if(item.Data.ItemType == ItemType.Accesory)
            {
                equipSlot = acessorySlot.GetComponent<EquipSlotUI>();
            }
            equipping = equipSlot.SetItem(item);
            if(equipping)
            {
                item = equipSlot.SwapItem(item);
                inventory.SetItem(Index,item);
            }
            else
            {
                inventory.Remove(Index);
            }
        }
        
        HidePanel();
        inventory.UpdateSlot(Index);
    }

    public void DropItem()
    {
        if(item is SubItem)
        {
            SubItem tempItem = item as SubItem;
            GameObject prefab = Instantiate(tempItem.subItemData.DropItemPrefab) as GameObject;
            prefab.transform.position = (Vector2)player.transform.position + new Vector2(player.transform.localScale.x*1.2f,1.0f);
            prefab.GetComponent<ItemPrefab>().SetDrop(tempItem.Amount);
        }
        else
        {
            EquipItem tempItem = item as EquipItem;
            GameObject prefab = Instantiate(tempItem.equipItemData.DropItemPrefab) as GameObject;
            prefab.transform.position = (Vector2)player.transform.position + new Vector2(player.transform.localScale.x*1.2f,1.0f);
        }
        inventory.Remove(Index);
        HidePanel();
        inventory.UpdateSlot(Index);
    }
    public void QuickSlotItem()
    {
        if(item.Data.ItemType == ItemType.Potion)
        {
            quickSlot = _quickSlotPotion.GetComponent<QuickSlotUI>();
        }
        else
        {
            quickSlot = _quickSlotWeapon.GetComponent<QuickSlotUI>();
        }
        quickSlot.SetQuickSlot(Index);
        HidePanel();
    }
    public void ViewEquipItem(Item equippedItem, ItemType type)
    {
        Index = -1;
        item = equippedItem;
        itemType = type;
        PopupImage.sprite = item.Data.IconSprite;
        PopupText.text = item.Data.Tooltip;
        PopupItemName.text = item.Data.Name;
        UseButtonText.fontSize = 13;
        UseButtonText.text = "UNEQUIP";
        SlotButton.SetActive(false);
        DropButton.SetActive(false);
    }
}
