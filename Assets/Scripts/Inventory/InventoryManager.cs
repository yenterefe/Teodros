using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab; // Prefab used to create inventory slots
    [SerializeField] private int initialSlotCount = 12; // Number of slots to initialize at start
    [SerializeField] private Transform slotsParent; // Parent transform where the slots will be placed

    private List<InventorySlot> inventorySlots = new List<InventorySlot>(); // List to store active inventory slots
    private Queue<InventorySlot> slotPool = new Queue<InventorySlot>(); // Pool for reusable inventory slots

    private void Awake()
    {
        // Initialize the pool of inventory slots with the specified count
        InitializeSlots(initialSlotCount);
    }

    private void OnEnable()
    {
        Inventory.OnInventoryChange += UpdateInventoryUI; // Subscribe to inventory change events
    }

    private void OnDisable()
    {
        Inventory.OnInventoryChange -= UpdateInventoryUI; // Unsubscribe to avoid memory leaks
    }

    private void UpdateInventoryUI(object sender, InventoryItem item)
    {
        // Call DrawInventory with the updated inventory
        DrawInventory(Inventory.GetCurrentInventory()); // Ensure this method fetches the current inventory
    }

    // Initializes a set number of inventory slots and adds them to the pool
    private void InitializeSlots(int count)
    {
        for (int i = 0; i < count; i++)
        {
            CreateInventorySlot();
        }
    }

    // Resets the inventory by deactivating all slots and returning them to the pool
    public void ResetInventory()
    {
        foreach (var slot in inventorySlots)
        {
            slot.gameObject.SetActive(false); // Hide the slot
            slotPool.Enqueue(slot); // Return the slot to the pool for future use
        }
        inventorySlots.Clear(); // Clear the active slots list
    }


    // Draws the inventory by assigning items to available slots
    public void DrawInventory(List<InventoryItem> inventory)
    {
        // Reset the current inventory before drawing new items
        ResetInventory();

        // Determine how many slots are needed based on the inventory size
        int requiredSlots = Mathf.Max(inventory.Count, initialSlotCount);

        // Ensure the pool has enough slots, create more if necessary
        while (slotPool.Count < requiredSlots)
        {
            CreateInventorySlot();
        }

        // Assign each inventory item to a slot
        for (int i = 0; i < inventory.Count; i++)
        {
            InventorySlot slot;
            if (slotPool.Count > 0)
            {
                slot = slotPool.Dequeue(); // Get a slot from the pool
            }
            else
            {
                slot = CreateInventorySlot(); // Create a new slot if the pool is empty
            }

            slot.gameObject.SetActive(true); // Make the slot visible
            slot.DrawSlot(inventory[i]); // Assign the inventory item to the slot
            inventorySlots.Add(slot); // Add the slot to the active slots list
        }

        // Optionally handle excess slots if there are fewer items than the initial slot count
        for (int i = inventory.Count; i < initialSlotCount; i++)
        {
            if (slotPool.Count > 0)
            {
                InventorySlot slot = slotPool.Dequeue();
                slot.gameObject.SetActive(true);
                slot.ResetSlot(); // Reset the slot if there are no items for it
                inventorySlots.Add(slot);
            }
            else
            {
                InventorySlot newSlot = CreateInventorySlot();
                newSlot.gameObject.SetActive(true);
                inventorySlots.Add(newSlot);
            }
        }
    }

    // Creates a new inventory slot, adds it to the pool, and returns it
    private InventorySlot CreateInventorySlot()
    {
        GameObject newSlotGO = Instantiate(slotPrefab, slotsParent); // Create a new slot GameObject
        InventorySlot newSlot = newSlotGO.GetComponent<InventorySlot>(); // Get the InventorySlot component
        newSlot.ResetSlot(); // Reset the slot to its default state
        slotPool.Enqueue(newSlot); // Add the slot to the pool for reuse
        return newSlot; // Return the newly created slot
    }
}