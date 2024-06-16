using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour, ICollectable
{
    public EventHandler OnHealthCollected;

    public void Collect()
    {
        if(OnHealthCollected != null)
        {
            OnHealthCollected(this, EventArgs.Empty);
        }
    }
}
