using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPrefab : MonoBehaviour
{
    public ItemData data;
    public InventoryController inventory;
    void Start()
    {
        inventory = GameObject.Find("InventoryController").GetComponent<InventoryController>();
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag=="Player")
        {
            inventory.Add(data);
            Destroy(gameObject);
        }
    }
}
