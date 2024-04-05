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
        
        //playerScript= GameObject.Find("Player").GetComponent<Player>();  

    }

    private void Start()
    {
       
    }

    private void Update()
    {
        transform.LookAt(player.transform);

    }
  

    public float SwordTimer()
    {
        return swordTimer;
    }

}
