using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat : ItemPrefab
{
    PlayerStatus stat;

    public override void ItemEffect(Item item, bool used)
    {
        stat = LinkPlayer(item);
        stat.setHunger(stat.getHunger() + stat.getMaxHunger()/2);
        sound.FxPlayWithClip(clip[0]);
    }
}
