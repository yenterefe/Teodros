using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI stackNumber;
    [SerializeField] private Image image;

    public InventoryItem currentItem;

    public EventHandler OnButtonClicked;

    public void ResetSlot()
    {
        itemName.enabled = false;
        stackNumber.enabled = false;
        image.enabled = false;
    }

    public void StartSlot()
    {
        itemName.enabled = true;
        stackNumber.enabled = true;
        image.enabled = true;
    }

    public void DrawSlot(InventoryItem item)
    {
        if (item == null)
        {
            ResetSlot();
        }

        StartSlot();

        itemName.text = item.ItemData.itemName;
        image.sprite = item.ItemData.sprite;
        stackNumber.text = item.StackSize.ToString();
        currentItem = item;
    }

    public void OnClick()
    {
        /*if(currentItem.ItemData.itemName.ToString() == "Ammo")
        {
            Debug.Log("Ammo");
        }

        if (currentItem.ItemData.itemName.ToString() == "Health")
        {
            Debug.Log("Health");
        }*/


        if(OnButtonClicked != null)
        {
            OnButtonClicked(this, EventArgs.Empty); 
        }
    }


    public string ItemNameMethod()
    {
        if (currentItem != null && currentItem.ItemData != null && itemName != null && itemName.text != null)
        {
            return currentItem.ItemData.itemName.ToString();
        }
        else
        {
            return string.Empty;
        }
    }
}