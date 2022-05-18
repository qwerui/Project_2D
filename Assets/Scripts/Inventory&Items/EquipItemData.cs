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

    [SerializeField] private int _value;
    [SerializeField] private int _red;
    [SerializeField] private int _blue;
    [SerializeField] private int _yellow;
    
    public override Item CreateItem(UnityEvent itemEvent)
    {
        return new EquipItem(this, itemEvent);
    }
}
