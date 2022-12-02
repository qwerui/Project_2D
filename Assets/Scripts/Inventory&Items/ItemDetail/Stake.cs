using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stake : ItemPrefab
{
    PlayerStatus stat;

    public override void ItemEffect(Item item, bool used) //스테이크 효과
    {
        stat = LinkPlayer(item);
        stat.setHunger(stat.getMaxHunger());
        sound.FxPlayWithClip(clip[0]);
    }
}
