using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerAccesory : ItemPrefab
{
    PlayerStatus stat;

    public override void ItemEffect(Item item, bool equip) //공복치 장신구 효과
    {
        stat = LinkPlayer(item);
        if(equip == true)
        {
            stat.setMaxHunger(stat.getMaxHunger()+(item.Data as EquipItemData).Value);
        }
        else
        {
            stat.setMaxHunger(stat.getMaxHunger()-(item.Data as EquipItemData).Value);
        }
    }
}
