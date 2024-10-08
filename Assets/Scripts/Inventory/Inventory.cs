using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    List<InventoryItem> inventory = new List<InventoryItem>();
    Dictionary<ItemData, InventoryItem> itemDictionary = new Dictionary<ItemData, InventoryItem>();

    private void OnEnable ()
    {
        // whenever bullets or health are picked up by the player the add function will be referenced so it can add the items in the game inventory
        Bullet.OnBulletCollected += Add;
        Health.OnHealthCollected += Add;
    }

    private void OnDisable()
    {
        Bullet.OnBulletCollected -= Add;
        Health.OnHealthCollected -= Add;
    }


    private void Add(object source, ItemData itemData)
    {
        // if the itemData or key already exists it will increase the stack size of the inventory item
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.AddToStack();
            Debug.Log($"you collected {itemData.itemName} and total stack is {item.StackSize}");
        }

        // if not it will create a new inventory item and create a new dictionary to update the stack size
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            itemDictionary.Add(itemData, newItem);
            Debug.Log($"you collected {itemData.itemName} and total stack is {newItem.StackSize}");
        }
    }
}
