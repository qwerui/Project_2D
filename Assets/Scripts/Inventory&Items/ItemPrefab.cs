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

    protected SoundDirector sound;
    protected PlayerController playerCtl;
    public AudioClip[] clip;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        GetComponent<SpriteRenderer>().sprite = data.IconSprite;
    }
    void Start()
    {
        inventory = GameObject.Find("InventoryController").GetComponent<InventoryController>();
        amount = amount==0?1:amount;
        Identify();
    }
    public void SetDrop(int Amount)
    {
        amount = Amount;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag=="Ground")
        {
            rigid.velocity = Vector2.zero;
        }
        if(other.gameObject.tag=="Player")
        {
            
            if(data.IsPassive == true)
            {
                player = other.gameObject;
                ItemEffect();
                if(data.ID > 3)
                {
                    GameObject.Find("GameDirector").GetComponent<GameDirector>().GetItemName(data.Name, true);
                }
                else
                {
                    DataDirector.Instance.resourceItem+=amount;
                }
                Destroy(gameObject);
            }
            else
            {
                int remain = inventory.Add(data,other.gameObject,amount);
                if(remain == 0)
                {
                    GameObject.Find("GameDirector").GetComponent<GameDirector>().GetItemName(data.Name, false);
                    Destroy(gameObject);
                }
            }
        }
    }
    // 아이템 효과 구현할 때 가장 먼저 호출해야함
    public PlayerStatus LinkPlayer(Item item = null)
    {
        if(player == null)
            player = item.GetPlayer();
        playerCtl = player.GetComponent<PlayerController>();
        sound = playerCtl.sound;
        return playerCtl.GetStat();
    }
    public abstract void ItemEffect(Item item = null, bool equip = true);
    public virtual void WeaponUse(){}
    public virtual void Identify(){}
}
