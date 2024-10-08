using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject slotObject;

    List<InventorySlot> inventorySlots = new List<InventorySlot>(12);

    // This class is responsible in creating and assigning each inventory in each slot

    public void ResetInventory()
    {
        foreach(Transform childrenObject in transform)
        {
            Destroy(childrenObject.gameObject);
        }

        inventorySlots= new List<InventorySlot>(12);
    }

    // Want to use our inventory item to be saved in the slots
    public void DrawInventory(List<InventoryItem>inventory)
    {
        // Will alaways refresh by resetting before adding any new item.
        ResetInventory();

        // create the slots based on the capacity
        for(int i = 0; i < inventorySlots.Capacity; i++)
        {
            // add inventory slot based on the capacity which is 12
            CreateInventorySlot();
        }

        //we save or create the inventory inside the slots
        for (int i = 0; i < inventory.Count; i++)
        {
            inventorySlots[i].DrawSlot(inventory[i]);
        }
    }

    public void CreateInventorySlot()
    {
      
        GameObject newSlot= Instantiate(slotObject);
        newSlot.transform.SetParent(transform);

        // getting the component or in this case the script 
        InventorySlot newSlotComponent = newSlot.GetComponent<InventorySlot>();

        //adding the component in the inventory slots
        inventorySlots.Add(newSlotComponent);   

        //resetting GUI 
        newSlotComponent.ResetSlot();

    }
}
