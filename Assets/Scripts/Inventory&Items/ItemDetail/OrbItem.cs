using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbItem : ItemPrefab
{
    PlayerStatus stat;

    public override void ItemEffect(Item item, bool used)
    {
        stat = LinkPlayer(item);
        switch(item.Data.ID)
        {
            case 112:
                stat.setAtk(stat.getAtk()+(item.Data as EquipItemData).Value);
                break;
            case 113:
                stat.setDef(stat.getDef()+(item.Data as EquipItemData).Value);
                break;
            case 114:
                stat.setMaxHp(stat.getMaxHp()+(item.Data as EquipItemData).Value);
                break;
            case 115:
                stat.setMaxHunger(stat.getMaxHunger()+(item.Data as EquipItemData).Value);
                break;
        }
    }
}
