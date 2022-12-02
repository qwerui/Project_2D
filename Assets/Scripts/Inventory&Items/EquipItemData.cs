using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EquipItemData", menuName = "Project_2D/EquipItemData", order = 2)]
public class EquipItemData : ItemData
{
    //장비 아이템 정보
    public int Value => _value;
    public int Red => _red;
    public int Blue => _blue;
    public int Yellow => _yellow;
    public int Rarity => _rarity;

    [SerializeField] private int _value;
    [SerializeField] private int _red;
    [SerializeField] private int _blue;
    [SerializeField] private int _yellow;
    [SerializeField] private int _rarity;
    
    public override Item CreateItem()
    {
        return new EquipItem(this);
    }
}
