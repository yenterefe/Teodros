using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
  

    private void OnCollisionEnter(Collision collision)
    {
        ICollectable collectable = collision.gameObject.GetComponent<ICollectable>();   

        if(collectable != null)
        {
            collectable.Collect();
        }
    }
}
