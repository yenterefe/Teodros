using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI stackNumber;
    public Image image;

    public void ResetSlot()
    {
        itemName.enabled = false;
        stackNumber.enabled = false;
        image.enabled = false;  
    }

    public void DrawSlot(InventoryItem item)
    {
        if(item == null)
        {
            ResetSlot();
        }

        itemName.enabled = true;
        stackNumber.enabled = true;
        image.enabled = true;

        itemName.text = item.ItemData.itemName;
        image.sprite = item.ItemData.sprite;
        stackNumber.text = item.StackSize.ToString();
    }

}
