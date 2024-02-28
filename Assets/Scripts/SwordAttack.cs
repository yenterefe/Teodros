using System;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    [SerializeField] GameObject sword;
    [SerializeField] GameObject inputManager;
    private GameObject player;
    private Player playerScript;

    [SerializeField] float cancelAttackSpeed;
    [SerializeField] float swordDamage = 0.5f;
    [SerializeField] float gunDamage = 0.75f;
    private float swordTimer = 0f; 

    private GameInput input;
   
    private string gameObjectName;

    private bool attack= false;
    private bool secondCombo;

    private float enemyLife = 5f;

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
        //input.OnLightAttackCanceled += AttackCanceled;
    }

    private void Update()
    {
        transform.LookAt(player.transform);

        //Debug.Log("attack is " + attack);

        //Debug.Log("enemy life is " + enemyLife);

        //Debug.Log("Is second combo active " + secondCombo);

        //Debug.Log( "Cancel speed is " + cancelAttackSpeed);

        bool takeDamage = playerScript.EnemyDamage();

        if (takeDamage)
        {
            enemyLife -= gunDamage;
        }

        if(enemyLife < 0)
        {
            //Debug.Log("Enemy is dead");
        }
    }
    // Have to refactor code to go to Enemy script 
    private void AttackPerformed(object receiver, EventArgs e)
    {
        attack = true;

        secondCombo = playerScript.SecondComboDamage();

        if (secondCombo == true)
        {
            cancelAttackSpeed = 2.4f;
        }

        //if (secondCombo != true)
        {
            //cancelAttackSpeed = 1.5f;
        }

        Invoke("CancelAttack", cancelAttackSpeed);
    }

    /*private void AttackCanceled(object receiver, EventArgs e)
    {
        // event cancel so fast that it remains false so I used invoke to slow it down 
        Invoke("CancelAttack", cancelSpeed);
    }*/

    private void CancelAttack()
    {
        attack=false;   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name== gameObjectName && attack == true)
        {
            enemyLife-=swordDamage;
            swordTimer += Time.deltaTime;   
        }
    }

    public float SwordTimer()
    {
        return swordTimer;
    }

}
