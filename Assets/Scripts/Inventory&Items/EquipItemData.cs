using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EquipItemData", menuName = "Project_2D/EquipItemData", order = 2)]
public class EquipItemData : ItemData
{
    public int Value => _value;
    public int Red => _red;
    public int blue => _blue;
    public int yellow => _yellow;
    public Sprite WeaponEffect => _weaponEffect;
    public Vector2 WeaponHitSize => _weaponHitSize;

    [SerializeField] private int _value;
    [SerializeField] private int _red;
    [SerializeField] private int _blue;
    [SerializeField] private int _yellow;
    [SerializeField] private Sprite _weaponEffect;
    [SerializeField] private Vector2 _weaponHitSize;
    
    public override Item CreateItem()
    {
        return new EquipItem(this);
    }
}
