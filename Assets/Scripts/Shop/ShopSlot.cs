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
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clip;

    ShopController shop_ctl;
    int index;

    //상점의 각 아이템 슬롯 초기화
    public void SetShopItem(ItemData item, ShopController _shop, int i)
    {
        shop_ctl = _shop;
        shopItem = item;
        slotImage.sprite = item.IconSprite;
        imageRect.localRotation = Quaternion.Euler(Vector3.zero);
        itemPrice.text = (item as SubItemData).Price.ToString();
        canBuyText.text = "●";
        index = i;
    }
    //아이템이 없는 경우
    public void SetNullItem()
    {
        itemPrice.text = "-";
        canBuyText.text="X";
    }
    //아이템 구매
    public void Buy()
    {
        if(shopItem == null)
        {
            return;
        }
        if(shop_ctl.BuyProcess(shopItem, index))
        {
            shopItem = null;
            slotImage.sprite = soldOut;
            imageRect.localRotation = Quaternion.Euler(0,0,20);
            itemPrice.text = "-";
            canBuyText.text="X";
            audioSource.PlayOneShot(clip);
        }
    }
}
