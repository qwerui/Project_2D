using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : ItemPrefab
{
    PlayerStatus stat;

    public override void ItemEffect(bool equip)
    {
        stat = LinkPlayer();
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
