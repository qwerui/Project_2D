using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ItemPrefab
{
    PlayerStatus stat;

    public override void ItemEffect(Item item, bool equip)
    {
        stat = LinkPlayer(item);
        if(equip == true)
        {
            stat.setAtk(stat.getAtk()+(item.Data as EquipItemData).Value);
        }
        else
        {
            stat.setAtk(stat.getAtk()-(item.Data as EquipItemData).Value);
        }
    }
}
