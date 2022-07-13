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
            if(data.ID > 3)
            {
                GameObject.Find("GameDirector").GetComponent<GameDirector>().GetItemName(data.Name);
            }
            if(data.ItemType == ItemType.Passive)
            {
                player = other.gameObject;
                ItemEffect();
                Destroy(gameObject);
            }
            else
            {
                int remain = inventory.Add(data,other.gameObject,amount);
                if(remain == 0)
                    Destroy(gameObject);
            }
            
        }
    }
    // 아이템 효과 구현할 때 가장 먼저 호출해야함
    public PlayerStatus LinkPlayer(Item item = null)
    {
        if(player == null)
            player = item.GetPlayer();
        return player.GetComponent<PlayerController>().GetStat();
    }
    public abstract void ItemEffect(Item item = null, bool equip = true);
    public virtual void WeaponUse(){}
}
