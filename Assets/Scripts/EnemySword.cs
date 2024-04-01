using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySword : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject playerHealthBar;

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
            playerHealthBar.GetComponent<Slider>().value -=20;        
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
