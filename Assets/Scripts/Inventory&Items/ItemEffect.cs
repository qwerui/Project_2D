using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect : MonoBehaviour
{
    PlayerStatus stat;

    // 모든 메소드의 처음에 들어가야 함
    // 플레이어 객체와 연결하는 메소드
    // stat이 비어있으면 Find하기 때문에 1회만 Find가 1회만 호출
    private void LinkPlayer()
    {
        if(stat==null)
            stat = GameObject.Find("Player").GetComponent<PlayerController>().GetStat();
    }

    public void RecoverHunger(int value)
    {
        LinkPlayer();
        stat.setHunger(stat.getHunger() + value);
    }

    public void AtkIncrease(int value)
    {
        LinkPlayer();
        stat.setAtk(stat.getAtk() + value);
    }
    public void AtkDecrease(int value)
    {
        LinkPlayer();
        stat.setAtk(stat.getAtk() - value);
    }
}
