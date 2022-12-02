using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ItemPrefab
{
    PlayerStatus stat;

    public override void ItemEffect(Item item, bool equip)//무기류 효과
    {
        stat = GameManager.Instance.stat;
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
