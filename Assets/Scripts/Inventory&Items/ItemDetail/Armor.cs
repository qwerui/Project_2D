using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : ItemPrefab
{
    PlayerStatus stat;

    public override void ItemEffect(Item item, bool equip)
    {
        stat = LinkPlayer(item);
        if(equip == true)
        {
            stat.setDef(stat.getDef()+(item.Data as EquipItemData).Value);
        }
        else
        {
            stat.setDef(stat.getDef()-(item.Data as EquipItemData).Value);
        }
    }
}