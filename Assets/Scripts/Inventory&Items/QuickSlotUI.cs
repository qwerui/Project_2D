using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotUI : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private Text _amountText;
    [SerializeField] private InventoryController inventory;
    [SerializeField] private DataController data;

    Item item;
    public bool HasItem => _iconImage.sprite != null;
    public int Index = -1;

    private GameObject _iconGo;
    private GameObject _textGo;
    //아이콘 on/off
    private void ShowIcon() => _iconGo.SetActive(true);
    private void HideIcon() => _iconGo.SetActive(false);
    //수량 on/off
    private void ShowText() => _textGo.SetActive(true);
    private void HideText() => _textGo.SetActive(false);

    private void Awake() {
        InitComponents();
    }
    private void Start() {
        if(DataDirector.Instance.isLoadedGame)
        {
            if(gameObject.name.Contains("Potion"))
            {
                Index = data.GetPotionIndex();
            }
            else
            {
                Index = data.GetWeaponIndex();
            }
            if(Index != -1)
            {
                SetQuickSlot(Index);
            }
        }
    }
    private void LateUpdate() {
        if(item != null)
            UpdateQuickSlot();
    }
    //퀵슬롯에 아이템 등록
    public void SetQuickSlot(int index)
    {
        Index = index;
        item = inventory.ReturnItem(Index);
        _iconImage.sprite = item.Data.IconSprite;
        SetItemAmount((item as SubItem).Amount);
        ShowIcon();
    }
    //퀵슬롯 초기화
    private void InitComponents()
    {
        // Game Objects
        _iconGo = transform.GetChild(0).gameObject;
        _textGo = _iconGo.transform.GetChild(0).gameObject;

        // Deactive Icon
        HideIcon();
    }
    //수량 업데이트
    void UpdateQuickSlot()
    {
        int amount = (item as SubItem).Amount;
        SetItemAmount(amount);
        if(amount<=0)
        {
            HideIcon();
            item = null;
            Index = -1;
        }
    }
    //수량 출력
    void SetItemAmount(int amount)
    {
        if (HasItem && amount > 1)
            ShowText();
        else
        {
            HideText();
        }
        _amountText.text = amount.ToString();
    }
    //퀵슬롯 사용
    public void QuickItemUse()
    {
        if(item != null)
        {
            (item as SubItem).Use();
            inventory.UpdateSlot(Index);
            UpdateQuickSlot();
        }
    }
}
