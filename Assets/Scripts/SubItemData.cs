using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SubItemData", menuName = "Project_2D/SubItemData", order = 3)]
public class SubItemData : ItemData {
      // 효과량(회복량 등)
    public float Value => _value;
    [SerializeField] private float _value;
    public override Item CreateItem()
    {
        return new SubItem(this);
    }
}