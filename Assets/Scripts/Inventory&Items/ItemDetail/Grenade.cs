using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : ItemPrefab
{
    PlayerStatus stat;
    public GrenadeEffect effectObj;

    public override void ItemEffect(Item item, bool used)
    {
        stat = LinkPlayer(item);
        GrenadeEffect effect = Instantiate(effectObj.gameObject).GetComponent<GrenadeEffect>();
        effect.SetEffect(player, stat);
        sound.FxPlayWithClip(clip[0]);
    }
}
