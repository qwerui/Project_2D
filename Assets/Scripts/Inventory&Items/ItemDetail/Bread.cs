using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bread : ItemPrefab
{
    PlayerStatus stat;

    public override void ItemEffect(bool used)
    {
        stat = LinkPlayer();
        stat.setHunger(stat.getHunger() + 10);
    }
}
