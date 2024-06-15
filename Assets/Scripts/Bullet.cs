using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, ICollectable
{
    public EventHandler OnBulletCollected;
 
    public void Collect()
    {
        if(OnBulletCollected != null)
        {
            OnBulletCollected(this, EventArgs.Empty);
        }
    }
}
