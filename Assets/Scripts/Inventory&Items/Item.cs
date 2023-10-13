using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item //아이템 실체 객체
{
    public ItemData Data { get; private set; }
    
    GameObject player;

    public Item(ItemData data) => Data = data;

    public void SetPlayer(GameObject _player) => player = _player;
    public GameObject GetPlayer(){ return player; }
}