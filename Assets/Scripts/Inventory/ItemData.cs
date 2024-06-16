using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Item data")]

public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
}
