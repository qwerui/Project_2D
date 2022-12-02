using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : InteractableObject
{
    public ItemData[] shopItem = new ItemData[8];
    ShopController shop_ctl;
    ShopUI shop_UI;

    private void Start() {
        if(!DataDirector.Instance.isLoadedGame) //이어하기가 아니면 랜덤 아이템 생성
            {
                for (int i = 0; i < 8; i++)
                {
                    shopItem[i] = ItemLoader.Instance.GetSubItemList()[Random.Range(0,ItemLoader.Instance.GetSubItemCount())];
                }
            }
    }

    protected override void Interaction() //시간을 멈추고 쇼핑 활성화
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
