using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdentifiedItem
{
    public int[] dataID;
    public int[] effect;
    public bool[] identified;

    public IdentifiedItem()
    {
        dataID = new int[6];
        effect = new int[6];
        identified = new bool[6];
        for(int i=0;i<6;i++)
        {
            dataID[i]=300+i;
            effect[i] = i;
        }
    }
    //알약 아이템 초기화
    public void Init()
    {
        for(int i=0;i<100;i++)
        {
            int index1 = Random.Range(0,6);
            int index2 = Random.Range(0,6);
            int temp = effect[index1];
            effect[index1] = effect[index2];
            effect[index2] = temp;
        }
    }
}
