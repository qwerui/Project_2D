using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SubItemData", menuName = "Project_2D/SubItemData", order = 3)]
public class SubItemData : ItemData {
    public override Item CreateItem(UnityEvent itemEvent)
    {
        return new SubItem(this, itemEvent);
    }
}