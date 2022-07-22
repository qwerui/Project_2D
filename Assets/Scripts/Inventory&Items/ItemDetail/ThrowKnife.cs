using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowKnife : ItemPrefab
{
    PlayerStatus stat;
    public ThrowKnifeEffect effectObj;

    public override void ItemEffect(Item item, bool used)
    {
        stat = LinkPlayer(item);
        ThrowKnifeEffect effect = Instantiate(effectObj.gameObject).GetComponent<ThrowKnifeEffect>();
        effect.SetEffect(player, stat);
        sound.FxPlayWithClip(clip[0]);
    }
}
