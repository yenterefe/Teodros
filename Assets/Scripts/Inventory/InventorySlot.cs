using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI stackNumber;
    [SerializeField] private Image image;

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
        if(item == null)
        {
            ResetSlot();
        }

        StartSlot();

        itemName.text = item.ItemData.itemName;
        image.sprite = item.ItemData.sprite;
        stackNumber.text = item.StackSize.ToString();
    }

}
