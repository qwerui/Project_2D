using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RandomCapsule : ItemPrefab
{
    public override void Identify() {
        int index = Array.IndexOf<int>(GameManager.Instance.identified.dataID, data.ID);
        if(GameManager.Instance.identified.identified[index] == false)
        {
            string temp = "";
            switch(data.ID)
            {
                case 300:
                    temp = "빨간 알약";
                    break;
                case 301:
                    temp = "파란 알약";
                    break;
                case 302:
                    temp = "초록 알약";
                    break;
                case 303:
                    temp = "노란 알약";
                    break;
                case 304:
                    temp = "보라 알약";
                    break;
                case 305:
                    temp = "청록 알약";
                    break;
            }
            data.ChangeTooltip(temp,"먹어보지 않으면 효과를 알 수 없다");
        }
    }
    public override void ItemEffect(Item item = null, bool equip = true)
    {
        int index = Array.IndexOf<int>(GameManager.Instance.identified.dataID, item.Data.ID);
        GameManager.Instance.identified.identified[index] = true;
        switch(GameManager.Instance.identified.effect[index])
        {
            case 0:
                GameManager.Instance.controller.ActiveBuff(AtkUp());
                item.Data.ChangeTooltip("힘의 알약", "공격력을 올려주는 알약이다");
                break;
            case 1:
                GameManager.Instance.controller.ActiveBuff(DefUp());
                item.Data.ChangeTooltip("강인함의 알약", "방어력을 올려주는 알약이다");
                break;
            case 2:
                GameManager.Instance.controller.ActiveBuff(Posion());
                item.Data.ChangeTooltip("독약", "최후의 수단으로 남겨두자");
                break;
            case 3:
                GameManager.Instance.controller.ActiveBuff(Cure());
                item.Data.ChangeTooltip("진통제", "아픔이 줄어든다");
                break;
            case 4:
                GameManager.Instance.stat.setHunger(GameManager.Instance.stat.getHunger()/2);
                item.Data.ChangeTooltip("소화제", "공복감이 빨리 드는 기분이다");
                break;
            case 5:
                item.Data.ChangeTooltip("전설의 비약", "먹으면 강해지는 기분이 든다");
                break;
        }
        GameManager.Instance.controller.sound.FxPlayWithClip(clip[0]);
    }
    IEnumerator AtkUp()
    {
        GameManager.Instance.stat.setAtk(GameManager.Instance.stat.getAtk()+5);
        yield return new WaitForSeconds(30);
        GameManager.Instance.stat.setAtk(GameManager.Instance.stat.getAtk()-5);
    }
    IEnumerator DefUp()
    {
        GameManager.Instance.stat.setDef(GameManager.Instance.stat.getDef()+5);
        yield return new WaitForSeconds(30);
        GameManager.Instance.stat.setDef(GameManager.Instance.stat.getDef()-5);
    }
    IEnumerator Posion()
    {
        int count = 0;
        while(count < 10)
        {
            GameManager.Instance.stat.setHp(GameManager.Instance.stat.getHp()-3);
            yield return new WaitForSeconds(1);
        }
    }
    IEnumerator Cure()
    {
        int count = 0;
        while(count < 10)
        {
            GameManager.Instance.stat.setHp(GameManager.Instance.stat.getHp()+3);
            yield return new WaitForSeconds(1);
        }
    }
}
