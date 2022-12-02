using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ItemData : ScriptableObject{
    //아이템 정보
    public int ID => _id;
    public string Name => _name;
    public string Tooltip => _tooltip;
    public Sprite IconSprite => _iconSprite;
    public ItemType ItemType => _itemType;
    public GameObject DropItemPrefab => _dropItemPrefab;
    public bool IsPassive => _isPassive;

    [SerializeField] private int      _id;
    [SerializeField] private string   _name;    // 아이템 이름
    [Multiline]
    [SerializeField] private string   _tooltip; // 아이템 설명
    [SerializeField] private Sprite   _iconSprite; // 아이템 아이콘
    [SerializeField] private ItemType _itemType; //아이템 종류
    [SerializeField] private GameObject _dropItemPrefab; // 바닥에 떨어질 때 생성할 프리팹
    [SerializeField] private bool _isPassive;

    /// 타입에 맞는 새로운 아이템 생성
    public abstract Item CreateItem();
    public void ChangeTooltip(string name, string tooltip)
    {
        _name = name;
        _tooltip = tooltip;
    }
}
public enum ItemType
{
    Weapon,
    Armor,
    Accesory,
    Potion,
    Subweapon,
    Others
}

