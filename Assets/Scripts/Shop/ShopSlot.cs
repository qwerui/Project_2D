using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    ItemData shopItem;
    [SerializeField] Sprite soldOut;
    [SerializeField] Image slotImage;
    [SerializeField] RectTransform imageRect;
    [SerializeField] Text canBuyText;
    [SerializeField] Text itemPrice;

    ShopController shop_ctl;

    public void SetShopItem(ItemData item, ShopController _shop)
    {
        shop_ctl = _shop;
        shopItem = item;
        slotImage.sprite = item.IconSprite;
        imageRect.localRotation = Quaternion.Euler(Vector3.zero);
        itemPrice.text = (item as SubItemData).Price.ToString();
        canBuyText.text = "●";
    }

    public void Buy()
    {
        if(shop_ctl.BuyProcess(shopItem))
        {
            shopItem = null;
            slotImage.sprite = soldOut;
            imageRect.localRotation = Quaternion.Euler(0,0,20);
            itemPrice.text = "-";
            canBuyText.text="X";
        }
    }
}
