using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlotUI : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private EquipItem item;
    [SerializeField] private ItemType type;
    [SerializeField] private ItemData FirstItem;
    [SerializeField] private GameObject _inventoryPopupObj;
    [SerializeField] private GameObject DataCtl;
    [SerializeField] private GameObject player;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;

    private InventoryPopupUI _inventoryPopupUI;
    private GameObject _iconGo;
    private DataController data;
    //아이콘 on/off
    private void ShowIcon() => _iconGo.SetActive(true);
    private void HideIcon() => _iconGo.SetActive(false);

    private void Awake() {
        _inventoryPopupUI = transform.parent.parent.parent.GetChild(1).gameObject.GetComponent<InventoryPopupUI>();
        _iconGo = transform.GetChild(0).gameObject;
        if(DataCtl != null)
            data = DataCtl.GetComponent<DataController>();
        HideIcon();
    }
    //아이템 정보 출력
    public void OpenItemPopup()
    {
        _inventoryPopupUI.ShowPanel();
        _inventoryPopupUI.ViewEquipItem(item, type);
    }
    //아이템 장비
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
    //아이템 해제
    public void RemoveItem()
    {
        _iconImage.sprite = null;
        item.UnEquip();
        item = null;
        HideIcon();
    }
    //아이템 스왑(아이템 장비 상태에서 인벤토리의 장비 아이템을 장비하는 경우)
    public Item SwapItem(Item newItem)
    {
        item.UnEquip();
        Item tempItem = item;
        item = newItem as EquipItem;
        _iconImage.sprite = newItem.Data.IconSprite;
        item.Equip();
        return tempItem;
    }
    public EquipItem GetItem()
    {
        return item as EquipItem;
    }
    //소리 출력
    public void OpenSound()
    {
        audioSource.PlayOneShot(clip);
    }
    //장비 아이템 불러오기
    public void LoadEquipItem()
    {
        if(DataDirector.Instance.isLoadedGame)
        {
            ItemData tempData = data.LoadEquipItem(type);
            if(tempData != null)
            {
                EquipItem tempItem = tempData.CreateItem() as EquipItem;
                tempItem.SetPlayer(player);
                SetItem(tempItem);
            }
        }
        else if(FirstItem != null)
        {
            EquipItem tempItem = FirstItem.CreateItem() as EquipItem;
            tempItem.SetPlayer(player);
            SetItem(tempItem); 
        }
    }
}
