using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbItem : ItemPrefab
{
    PlayerStatus stat;

    public override void ItemEffect(Item item, bool used)
    {
        stat = GameManager.Instance.stat;
        switch(this.data.ID)
        {
            case 112:
                stat.setAtk(stat.getAtk()+(this.data as EquipItemData).Value);
                break;
            case 113:
                stat.setDef(stat.getDef()+(this.data as EquipItemData).Value);
                break;
            case 114:
                stat.setMaxHp(stat.getMaxHp()+(this.data as EquipItemData).Value);
                break;
            case 115:
                stat.setMaxHunger(stat.getMaxHunger()+(this.data as EquipItemData).Value);
                break;
        }
    }
}
