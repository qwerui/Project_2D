using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPotion : ItemPrefab
{
    PlayerStatus stat;

    public override void ItemEffect(Item item, bool used)
    {
        stat = LinkPlayer(item);
        switch(item.Data.ID) //hp포션류 효과
        {
            case 205:
                stat.setHp(stat.getHp()+stat.getMaxHp()/10);
                break;
            case 206:
                stat.setHp(stat.getHp()+stat.getMaxHp()/4);
                break;
            case 207:
                stat.setHp(stat.getHp()+stat.getMaxHp()/2);
                break;
            case 208:
                stat.setHp(stat.getMaxHp());
                break;
            case 209:
                stat.setHp(stat.getHp()+stat.getMaxHp()/10);
                stat.setHunger(stat.getHunger()+stat.getMaxHunger()/10);
                break;
            case 210:
                stat.setHp(stat.getHp()+stat.getMaxHp()/4);
                stat.setHunger(stat.getHunger()+stat.getMaxHunger()/4);
                break;
            case 211:
                stat.setHp(stat.getHp()+stat.getMaxHp()/2);
                stat.setHunger(stat.getHunger()+stat.getMaxHunger()/2);
                break;
            case 212:
                stat.setHp(stat.getMaxHp());
                stat.setHunger(stat.getMaxHunger());
                break;
        }
        sound.FxPlayWithClip(clip[0]);
    }
}
