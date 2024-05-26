using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSword : MonoBehaviour
{
    [SerializeField] private GameObject enemyHealthBar;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject inputManager;
    [SerializeField] private GameObject enemy;

    private GameInput gameInput;
    private bool isShieldActive;
    private bool isPlayerAttacking = false;
    private bool isEnemyHit = false;

    private Camera cam;

    private const string HIT = "Hit";
    private const string IDLE = "Idle";

    // Don't delete
    //[SerializeField] private ParticleSystem sparkle;
    //[SerializeField] private ParticleSystem blood;


    private void Awake()
    {
        gameInput =inputManager.GetComponent<GameInput>();

        cam = Camera.main;
    }

    private void Start()
    {
        gameInput.OnLightAttackPerformed += ManageAttack;
        gameInput.OnLightAttackCanceled += ManageCancelAttack;
    }

    private void FixedUpdate()
    {
        enemyHealthBar.transform.rotation = cam.transform.rotation;
      
    }

    private void ManageAttack(object receiver, EventArgs  e)
    {
        isPlayerAttacking = true;
    }

    private void ManageCancelAttack(object receiver, EventArgs e)
    {
        isPlayerAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (animator.isActiveAndEnabled ==true)
        {
            isShieldActive = animator.GetBehaviour<BlockStateA>().Blocking();
        }
      
        if (other.gameObject.CompareTag("Enemy") && !isShieldActive && isPlayerAttacking)
        {
            isEnemyHit = true;
            enemyHealthBar.GetComponent<Slider>().value -=20;

            // blood.Play();
        }

        if (other.gameObject.CompareTag("Shield") && isShieldActive)
        {
            isEnemyHit = false;

            // Don't delete
            // sparkle.Play();

            // Don't delete
            //play block animation
        }
    }

    private void OnTriggerExit(Collider other)
    {
        animator.SetBool( IDLE, true);
        isEnemyHit=false;
    }

    public bool IsEnemyHit()
    {
        return isEnemyHit;  
    }
}
