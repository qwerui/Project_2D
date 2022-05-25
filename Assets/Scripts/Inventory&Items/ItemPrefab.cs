using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemPrefab : MonoBehaviour
{
    public ItemData data;
    public InventoryController inventory;
    public Rigidbody2D rigid;
    public int amount;

    public GameObject player;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        inventory = GameObject.Find("InventoryController").GetComponent<InventoryController>();
        amount = amount==0?1:amount;
    }
    public void SetDrop(int Amount)
    {
        amount = Amount;
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag=="Ground")
        {
            rigid.velocity = Vector2.zero;
        }
        if(other.gameObject.tag=="Player")
        {
            if(data.ItemType == ItemType.Passive)
            {
                ItemEffect();
                Destroy(gameObject);
            }
            else
            {
                int remain = inventory.Add(data,amount);
                if(remain == 0)
                    Destroy(gameObject);
            }
            
        }
    }
    public PlayerStatus LinkPlayer()
    {
        if(player == null)
            player = GameObject.Find("Player");
        return player.GetComponent<PlayerController>().GetStat();
    }
    public abstract void ItemEffect(bool equip = true);
}
