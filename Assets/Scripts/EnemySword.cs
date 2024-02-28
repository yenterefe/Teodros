using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;

    private float enemySwordTimer = 0;

    private PlayerAnimation playerAnimation;

    private void Awake()
    {
        playerAnimation= playerPrefab.GetComponent<PlayerAnimation>();  
    }

    private void OnTriggerEnter(Collider other)
    {
        bool shieldActive = playerAnimation.ShieldActive();

        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player take damage");
        }

        else if(other.gameObject.CompareTag("Shield") && shieldActive ==true)
        {
            enemySwordTimer += Time.deltaTime;
            //Debug.Log($"Player Blocked and the timer speed is {enemySwordTimer}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        enemySwordTimer = 0;
    }


    public float EnemySwordTimer()
    {
        return enemySwordTimer;
    }
}
