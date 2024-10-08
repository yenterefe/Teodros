using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]

// this class will create an instance for each inventory item based on the item data. Without this, all items will be the same.
// It will also create a stack size for the each iventory items.

public class InventoryItem
{
    private ItemData itemData;
    private int stackSize;

    public ItemData ItemData 
    { 
        get { return itemData; }
        set { itemData = value; } 
    }

    public int StackSize
    {
        get { return stackSize; }
        set { stackSize = value; }
    }


    public InventoryItem(ItemData itemData)
    {
        ItemData = itemData;

        // If we do not add this line it wont add to one after we collect an inventory item in the scene
        AddToStack();
    }

    public void AddToStack()
    {
        StackSize++;
    }

    public void RemoveFromStack()
    {
        StackSize--;
    }
}
