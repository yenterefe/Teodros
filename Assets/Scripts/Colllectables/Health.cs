using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour, ICollectable
{
    public static EventHandler<ItemData> OnHealthCollected;
    [SerializeField] ItemData healthData;

    public void Collect()
    {
        if(OnHealthCollected != null)
        {
            OnHealthCollected(this, healthData);
            gameObject.SetActive(false);
        }
    }
}
