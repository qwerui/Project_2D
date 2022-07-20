using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bread : ItemPrefab
{
    PlayerStatus stat;

    public override void ItemEffect(Item item, bool used)
    {
        stat = LinkPlayer(item);
        stat.setHunger(stat.getHunger() + stat.getMaxHunger()/10);
        sound.FxPlayWithClip(clip[0]);
    }
}
