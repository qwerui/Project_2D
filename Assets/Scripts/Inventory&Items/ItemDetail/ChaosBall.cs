using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosBall : ItemPrefab
{
    public override void ItemEffect(Item item = null, bool equip = true)
    {
        int index = Random.Range(0,7);
        switch(index)
        {
            case 0:
                GameManager.Instance.stat.setAtk(GameManager.Instance.stat.getAtk()+Random.Range(-10,11));
                break;
            case 1:
                GameManager.Instance.stat.setDef(GameManager.Instance.stat.getDef()+Random.Range(-3,4));
                break;
            case 2:
                GameManager.Instance.stat.setMaxHp(GameManager.Instance.stat.getMaxHp()+(Random.Range(-6,7)*5));
                break;
            case 3:
                GameManager.Instance.stat.setMaxHunger(GameManager.Instance.stat.getMaxHunger()+(Random.Range(-6,7)*5));
                break;
            case 4:
                GameManager.Instance.stat.setHunger(GameManager.Instance.stat.getMaxHunger());
                GameManager.Instance.stat.setHp(GameManager.Instance.stat.getMaxHp());
                break;
            case 5:
                GameManager.Instance.stat.setHunger(GameManager.Instance.stat.getHunger()/10);
                GameManager.Instance.stat.setHp(GameManager.Instance.stat.getHp()/10);
                break;
            case 6:
                GameManager.Instance.controller.GainExprience(GameManager.Instance.stat.getMaxExperience());
                break;
        }
    }
}
