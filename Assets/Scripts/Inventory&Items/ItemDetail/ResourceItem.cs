using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceItem : ItemPrefab
{
    PlayerStatus stat;
    //자원 아이템 효과
    public override void ItemEffect(Item item, bool equip)
    {
        stat = LinkPlayer(item);
        if(this.data.ID == 0)
        {
            stat.setRedBall(stat.getRedBall() + amount);
        }
        else if(this.data.ID == 1)
        {
            stat.setBlueBall(stat.getBlueBall() + amount);
        }
        else if(this.data.ID == 2)
        {
            stat.setYellowBall(stat.getYellowBall() + amount);
        }
        else if(this.data.ID == 3)
        {
            stat.setGold(stat.getGold() + amount);
        }
    }
}
