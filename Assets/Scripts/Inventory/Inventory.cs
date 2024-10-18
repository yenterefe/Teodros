using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    public static EventHandler<InventoryItem> OnInventoryChange; // Event to notify inventory changes
    private static List<InventoryItem> inventory = new List<InventoryItem>(); // Static list to access globally
    private static Dictionary<ItemData, InventoryItem> itemDictionary = new Dictionary<ItemData, InventoryItem>();

    [SerializeField] GameObject gameManagerObj;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = gameManagerObj.GetComponent<GameManager>();
    }

    private void OnEnable()
    {
        // Subscribe to events for collecting bullets and health
        Bullet.OnBulletCollected += Add;
        Health.OnHealthCollected += Add;
        gameManager.OnBulletUIPressed += Remove;
    }

    private void OnDisable()
    {
        // Unsubscribe from events to avoid memory leaks
        Bullet.OnBulletCollected -= Add;
        Health.OnHealthCollected -= Add;
        gameManager.OnBulletUIPressed -= Remove;
    }

    // Method to get the current inventory
    public static List<InventoryItem> GetCurrentInventory()
    {
        return inventory; // Return the static inventory list
    }

    private void Add(object source, ItemData itemData)
    {
        // If the itemData or key already exists, it will increase the stack size of the inventory item
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.AddToStack();
            //Debug.Log($"You collected {itemData.itemName} and total stack is {item.StackSize}");
            OnInventoryChange?.Invoke(this, item); // Notify the change with the updated item
        }
        else
        {
            // If not, it will create a new inventory item and add it to the dictionary
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            itemDictionary.Add(itemData, newItem);
            //Debug.Log($"You collected {itemData.itemName} and total stack is {newItem.StackSize}");
            OnInventoryChange?.Invoke(this, newItem); // Notify the change with the new item
        }
    }

    private void Remove(object source, ItemData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.RemoveFromStack();
            if (item.StackSize == 0)
            {
                inventory.Remove(item);
                itemDictionary.Remove(itemData);
            }

            OnInventoryChange?.Invoke(this, item); // Notify the change with the removed item
        }
    }
}
