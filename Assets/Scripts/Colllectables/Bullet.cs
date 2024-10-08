using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, ICollectable
{
    // making it static will be able to count for all bullets stack and we wont need to create an instance for each bullet.
    // We are also passing the item data to be able to create an inventory item. 

    public static EventHandler<ItemData> OnBulletCollected;

    [SerializeField] private ItemData bullet;

    public void Collect()
    {
        if(OnBulletCollected != null)
        {
            OnBulletCollected(this, bullet);
            gameObject.SetActive(false);
        }
    }
}
