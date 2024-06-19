using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, ICollectable
{
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
