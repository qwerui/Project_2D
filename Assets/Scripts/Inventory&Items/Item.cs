using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    public ItemData Data { get; private set; }
    
    GameObject player;

    public Item(ItemData data) => Data = data;

    public void SetPlayer(GameObject _player) => player = _player;
    public GameObject GetPlayer(){ return player; }
}