using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] private GameObject shopSlotGroup;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject Shop_UI;
    [SerializeField] private GameObject Inventory;
    private ItemData[] subItemList;

    ShopUI shopUI;
    PlayerStatus stat;
    InventoryController inventory;
    Shop shop;
    
    void Start()
    {
        subItemList = Resources.LoadAll<ItemData>("ItemData/SubItem");
        shopUI = Shop_UI.GetComponent<ShopUI>();
        stat = GameManager.Instance.stat;
        inventory = Inventory.GetComponent<InventoryController>();
    }
    public ItemData GetRandomSubItem()
    {
        return subItemList[Random.Range(0,subItemList.Length)];
    }
    public void SetShopSlots(ItemData[] item, Shop shop)
    {
        this.shop = shop;
        shopUI.SetPlayerGold(stat.getGold());
        for(int i=0;i<8;i++)
        {
            if(item[i] != null)
            {
                shopSlotGroup.transform.GetChild(i).gameObject.GetComponent<ShopSlot>().SetShopItem(item[i], this, i);
            }
            else
            {
                shopSlotGroup.transform.GetChild(i).gameObject.GetComponent<ShopSlot>().SetNullItem();
            }
        }
    }
    public bool BuyProcess(ItemData item, int index)
    {
        int gold = stat.getGold() - (item as SubItemData).Price;
        if(gold < 0)
            return false;
        shop.shopItem[index] = null;
        stat.setGold(gold);
        shopUI.SetPlayerGold(gold);
        inventory.Add(item, player);
        return true;
    }
    
}
