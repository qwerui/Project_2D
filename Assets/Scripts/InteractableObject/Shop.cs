using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : InteractableObject
{
    public ItemData[] shopItem = new ItemData[8];
    ShopController shop_ctl;
    ShopUI shop_UI;

    private void Start() {
        if(!DataDirector.Instance.isLoadedGame)
            {
                for (int i = 0; i < 8; i++)
                {
                    shopItem[i] = ItemLoader.Instance.GetSubItemList()[Random.Range(0,ItemLoader.Instance.GetSubItemCount())];
                }
            }
    }

    protected override void Interaction()
    {
        if(shop_ctl == null)
        {
            shop_ctl = interactionDirector.shopController.GetComponent<ShopController>();
            shop_UI = interactionDirector.shopUI.GetComponent<ShopUI>();    
        }
        shop_ctl.SetShopSlots(shopItem, this);
        interactionDirector.MainUI.SetActive(false);
        interactionDirector.shopUI.SetActive(true);
        Time.timeScale = 0;
    }
    protected override void ExitAction()
    {

    }
}
