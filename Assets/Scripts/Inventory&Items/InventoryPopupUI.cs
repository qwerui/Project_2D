using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopupUI : MonoBehaviour
{
    Item item;
    InventoryController inventory;

    Image PopupImage;
    Text PopupText;
    Text PopupItemName;
    Text UseButtonText;
    GameObject SlotButton;
    QuickSlotUI quickSlot;

    [SerializeField] GameObject _inventory;
    [SerializeField] GameObject player;
    [SerializeField] GameObject _quickSlotPotion;
    [SerializeField] GameObject _quickSlotWeapon;

    int Index;

    public void ShowPanel() => gameObject.SetActive(true);
    public void HidePanel() => gameObject.SetActive(false);

    private void Awake() {
        inventory = _inventory.GetComponent<InventoryController>();
        PopupImage = transform.GetChild(0).gameObject.GetComponent<Image>();
        PopupText = transform.GetChild(1).gameObject.GetComponent<Text>();
        PopupItemName = transform.GetChild(2).gameObject.GetComponent<Text>();
        SlotButton = transform.GetChild(4).gameObject;
        UseButtonText = transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
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
        PopupText.text = item.Data.Tooltip;
        if(item is SubItem)
        {
            PopupItemName.text = item.Data.Name+"  (Stock : "+(item as SubItem).Amount+" )";
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
        if(item is SubItem)
        {
            (item as SubItem).Use();
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
            prefab.GetComponent<SubItemPrefab>().SetDrop(tempItem.Amount);
            tempItem.SetAmount(0);
        }
        HidePanel();
        inventory.UpdateSlot(Index);
    }
    public void QuickSlotItem()
    {
        if(item.Data.ItemType == ItemType.Potion)
        {
            quickSlot = _quickSlotPotion.GetComponent<QuickSlotUI>();
            quickSlot.SetQuickSlot(Index);
        }
        else
        {
            quickSlot = _quickSlotWeapon.GetComponent<QuickSlotUI>();
            quickSlot.SetQuickSlot(Index);
        }
        HidePanel();
    }
}
