using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SubItemPrefab : MonoBehaviour
{
    public ItemData data;
    public InventoryController inventory;
    public UnityEvent itemEvent;
    public Rigidbody2D rigid;
    public int amount;

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
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag=="Player")
        {
            inventory.Add(data, itemEvent,amount);
            Destroy(gameObject);
        }
    }
}
