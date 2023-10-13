using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SubItemData", menuName = "Project_2D/SubItemData", order = 3)]
public class SubItemData : ItemData {
    //소비 아이템 정보
    public int Price => _price;
    [SerializeField] private int _price;
    public override Item CreateItem()
    {
        return new SubItem(this);
    }
}