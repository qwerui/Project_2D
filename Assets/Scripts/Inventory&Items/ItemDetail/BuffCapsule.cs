using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffCapsule : ItemPrefab
{
    PlayerStatus stat;

    public override void ItemEffect(Item item, bool used)
    {
        stat = LinkPlayer(item);
        switch (item.Data.ID)
        {
            case 213:
                playerCtl.ActiveBuff(BuffAtk());
                break;
            case 214:
                playerCtl.ActiveBuff(BuffDef());
                break;
        }
        sound.FxPlayWithClip(clip[0]);
    }
    IEnumerator BuffAtk()
    {
        stat.setAtk(stat.getAtk() + 10);
        yield return new WaitForSeconds(30);
        stat.setAtk(stat.getAtk() - 10);
    }
    IEnumerator BuffDef()
    {
        stat.setDef(stat.getDef() + 5);
        yield return new WaitForSeconds(30);
        stat.setAtk(stat.getDef() - 5);
    }
}
