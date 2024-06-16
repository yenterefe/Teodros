using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private bool isEnemyTriggerEntered = false;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            isEnemyTriggerEntered = true;
            Debug.Log("player has entered in the trigger box");
        }
    }

    public bool IsEnemyTriggerEntered()
    {
        return isEnemyTriggerEntered;
    }
}
