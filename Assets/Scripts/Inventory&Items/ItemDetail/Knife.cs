using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : ItemPrefab
{
    PlayerStatus stat;

    public override void ItemEffect(Item item, bool equip)
    {
        stat = LinkPlayer(item);
        if(equip == true)
        {
            stat.setAtk(stat.getAtk() + 5);
        }
        else
        {
            stat.setAtk(stat.getAtk() - 5);
        }
    }
}
