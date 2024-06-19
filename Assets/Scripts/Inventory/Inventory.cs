using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    List<InventoryItem> inventory = new List<InventoryItem>();
    Dictionary<ItemData, InventoryItem> itemDictionary = new Dictionary<ItemData, InventoryItem>();

    private void Start()
    {
        Bullet.OnBulletCollected += Add;
    }

    /*private void OnEnable()
    {
        Bullet.OnBulletCollected += Add;
    }*/


    private void Add(object source, ItemData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.AddToStack();
            Debug.Log($"you collected {itemData.itemName} and total stack is {item.stack}");
        }


        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            itemDictionary.Add(itemData, newItem);
            Debug.Log($"you collected {itemData.itemName} and total stack is {newItem.stack}");
        }
    }
}
