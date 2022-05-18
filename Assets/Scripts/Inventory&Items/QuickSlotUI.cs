using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotUI : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private Text _amountText;
    [SerializeField] private InventoryController inventory;

    Item item;
    public bool HasItem => _iconImage.sprite != null;
    int Index;

    private GameObject _iconGo;
    private GameObject _textGo;

    private void ShowIcon() => _iconGo.SetActive(true);
    private void HideIcon() => _iconGo.SetActive(false);

    private void ShowText() => _textGo.SetActive(true);
    private void HideText() => _textGo.SetActive(false);

    private void Awake() {
        InitComponents();
    }
    private void LateUpdate() {
        if(item != null)
            UpdateQuickSlot();
    }
    public void SetQuickSlot(int index)
    {
        Index = index;
        item = inventory.ReturnItem(Index);
        _iconImage.sprite = item.Data.IconSprite;
        SetItemAmount((item as SubItem).Amount);
        ShowIcon();
    }

    private void InitComponents()
    {
        // Game Objects
        _iconGo = transform.GetChild(0).gameObject;
        _textGo = _iconGo.transform.GetChild(0).gameObject;

        // Deactive Icon
        HideIcon();
    }
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
    public void QuickItemUse()
    {
        (item as SubItem).Use();
        inventory.UpdateSlot(Index);
    }
}