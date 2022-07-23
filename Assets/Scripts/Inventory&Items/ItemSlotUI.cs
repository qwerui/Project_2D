using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private Text _amountText;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;

    public int Index { get; private set; }
    public bool HasItem => _iconImage.sprite != null;

    private InventoryPopupUI _inventoryPopupUI;

    private GameObject _iconGo;
    private GameObject _textGo;

    public void SetSlotIndex(int index) => Index = index;

    private void ShowIcon() => _iconGo.SetActive(true);
    private void HideIcon() => _iconGo.SetActive(false);

    private void ShowText() => _textGo.SetActive(true);
    private void HideText() => _textGo.SetActive(false);

    private void Awake() {
        InitComponents();
    }

    private void InitComponents()
    {
        _inventoryPopupUI = transform.parent.parent.parent.GetChild(1).gameObject.GetComponent<InventoryPopupUI>();

        // Game Objects
        _iconGo = transform.GetChild(0).gameObject;
        _textGo = _iconGo.transform.GetChild(0).gameObject;

        // Deactive Icon
        HideIcon();
    }
    // 아이템 개수 출력
    public void SetItemAmount(int amount)
    {
        if (HasItem && amount > 1)
            ShowText();
        else
            HideText();

        _amountText.text = amount.ToString();
    }
    // 슬롯에 아이템 등록 
    public void SetItem(Sprite itemSprite)
    {
        if (itemSprite != null)
        {
            _iconImage.sprite = itemSprite;
            ShowIcon();
        }
        else
        {
            RemoveItem();
        }
    }
    // 슬롯에서 아이템 제거
    public void RemoveItem()
    {
        _iconImage.sprite = null;
        HideIcon();
        HideText();
    }

    public void OpenItemPopup()
    {
        _inventoryPopupUI.ShowPanel();
        _inventoryPopupUI.SetPopupItem(Index);
    }
    public void OpenSound()
    {
        audioSource.PlayOneShot(clip);
    }
}
