using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pasta : ItemPrefab
{
    PlayerStatus stat;

    public override void ItemEffect(Item item, bool used)
    {
        stat = LinkPlayer(item);
        stat.setHunger(stat.getHunger() + stat.getMaxHunger()/4);
        sound.FxPlayWithClip(clip[0]);
    }
}
