using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]

public class InventoryItem
{
    public ItemData itemdData;
    public int stack;

    public InventoryItem(ItemData itemData)
    {
        this.itemdData = itemData;
        AddToStack();
    }

    public void AddToStack()
    {
        stack++;
    }


    public void RemoveFromStack()
    {
        stack--;
    }
}
