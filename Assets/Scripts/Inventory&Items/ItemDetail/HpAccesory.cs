using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpAccesory : ItemPrefab
{
    PlayerStatus stat;

    public override void ItemEffect(Item item, bool equip)
    {
        stat = LinkPlayer(item);
        if(equip == true)
        {
            stat.setMaxHp(stat.getMaxHp()+(item.Data as EquipItemData).Value);
        }
        else
        {
            stat.setMaxHp(stat.getMaxHp()-(item.Data as EquipItemData).Value);
        }
    }
}
