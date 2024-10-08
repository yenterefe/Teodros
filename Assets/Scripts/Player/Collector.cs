using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //player checks to see if there is a collision with any object with the interface ICollectable
        ICollectable collectable = collision.gameObject.GetComponent<ICollectable>();   

        if(collectable != null)
        {
            collectable.Collect();
        }
    }
}
