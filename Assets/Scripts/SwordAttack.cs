using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    [SerializeField] GameObject sword;
    [SerializeField] GameObject inputManager;
    private GameObject player;
    private Player playerScript;

    [SerializeField] float cancelSpeed = 0.75f;
    [SerializeField] float swordDamage = 0.5f;
    [SerializeField] float gunDamage = 0.75f;

    private GameInput input;
   
    private string gameObjectName;

    private bool attack= false;

    private float enemyLife = 5;

    private void Awake()
    {

       GetComponent<Rigidbody>();  
       gameObjectName = sword.name;

        input=inputManager.GetComponent<GameInput>();

        player = GameObject.Find("Player");
        
        playerScript= GameObject.Find("Player").GetComponent<Player>();  

    }

    private void Start()
    {
        
        input.OnLightAttackPerformed += AttackPerformed;
        input.OnLightAttackCanceled += AttackCanceled;
    }

    private void Update()
    {
        transform.LookAt(player.transform);

        Debug.Log(enemyLife);

        bool takeDamage = playerScript.EnemyDamage();

        if (takeDamage)
        {
            enemyLife -= gunDamage;
        }

        if(enemyLife < 0)
        {
            Debug.Log("Enemy is dead");
        }
    }

    private void AttackPerformed(object receiver, EventArgs e)
    {
        attack = true;
    }

    private void AttackCanceled(object receiver, EventArgs e)
    {
        // event cancel so fast that it remains false so I used invoke to slow it down 
        Invoke("CancelAttack", cancelSpeed);
    }

    private void CancelAttack()
    {
        attack=false;   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name== gameObjectName && attack == true)
        {
            enemyLife-=swordDamage;
        }
    }

}
