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

    private Camera cam;


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

    private void Update()
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
            enemyHealthBar.GetComponent<Slider>().value -=20;

            // play hit animation 

            // blood.Play();
        }

        if (other.gameObject.CompareTag("Shield") && isShieldActive)
        {
            // Don't delete
            // sparkle.Play();

            // Don't delete
            //play block animation
        }
    }
}
