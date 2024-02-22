using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    //Player Input
    [SerializeField] private GameObject inputManger;
    [SerializeField] private float directionSpeed = 2;
    private Vector3 moveDir;
    private Vector3 direction;
    private GameInput gameInput;

    //Animation
    [SerializeField] private float animationSmoothTime = .002f;
    [SerializeField] GameObject animationObject;
    private PlayerAnimation playerAnimation;
    private Vector2 currentAnimationBlendVector;
    private Vector2 animationVelocity;
    private float animationTimer = 2f;
    private float secondAttackAnimationTimer = 2.4f;

    //Attack
    [SerializeField] private float delayShotFired = 0.5f;
    private bool lightAttack;
    private bool activateSecondCombo = false;
    private float timer = 0;
    private float buttonPressed = 0;
    private bool shotFired= false;
    private bool enemyTakeDamage= false;
    private bool secondCombo; 

    [SerializeField] private LayerMask aimColliderMask = new LayerMask();

    // Camera
    private Camera cam;

    // RigidBody
    private Rigidbody playerRigidbody;

    //Aim
    [SerializeField] private GameObject aim;
    [SerializeField] private GameObject noEnemyCrossHair;
    [SerializeField] private GameObject enemyCrossHair;
    [SerializeField] private GameObject rifle;
    [SerializeField] private GameObject target;
    //[SerializeField] private Transform debugTransform;
    private string gameObjectName;

    private void Awake()
    {
        gameInput = inputManger.GetComponent<GameInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimation = animationObject.GetComponent<PlayerAnimation>();
        cam = Camera.main;
    }

    private void Start()
    {
        gameInput.OnLightAttackPerformed += LightAttackPerformed;
        gameInput.OnRifleFirePerformed += ShotFiredPerformed;
        gameInput.OnRifleFireCanceled += ShotNotFired;
    }

    private void Update()
    {
        Debug.Log("Is shot fired " + shotFired);
        Debug.Log("Name of object aimed at " + gameObjectName);


    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    /*private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "Rifle")
        {
            rifleBool = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Rifle")
        {
            rifleBool = false;
        }
    }*/

    public Vector3 Movement()
    {
        return moveDir;
    }

    private void LightAttackPerformed(object subscriber, EventArgs e)
    {
        buttonPressed++;
        lightAttack = true;
    }

    private void ShotFiredPerformed(object receiver, EventArgs e)
    {
        shotFired = true;
    }

    private void ShotNotFired(object receiver, EventArgs e)
    {
        Invoke("DelayShotCancel", delayShotFired);
    }

    private void DelayShotCancel()
    {
        shotFired = false;
    }

    public Vector2 SmoothDumpAnimation()
    {
        return currentAnimationBlendVector;
    }

    // See if you need this later
    public bool SecondCombo()
    {
        return activateSecondCombo;
    }

    private void HandleMovement()
    {
        bool isAiming = playerAnimation.IsPlayerAiming();

        {
            Vector2 inputDirection = gameInput.Player2DirectionNormalized();

            // To smoothen corner directions for movement
            currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, inputDirection, ref animationVelocity, animationSmoothTime);

            //Vector3 vector3Direction= new Vector3(inputDirection.x, 0, inputDirection.y);

            Vector3 vector3Direction = new Vector3(currentAnimationBlendVector.x, 0, currentAnimationBlendVector.y);

            vector3Direction = vector3Direction.normalized;

            Vector3 camForward = cam.transform.forward;
            Vector3 camgetRight = cam.transform.right;

            camForward.y = 0;
            camForward.y = 0;

            Vector3 forwardRelative = vector3Direction.z * camForward;
            Vector3 rightRelative = vector3Direction.x * camgetRight;

            moveDir = forwardRelative + rightRelative;
        }

        if (isAiming == true)
        {
            // Player takes camera totation
            transform.rotation = cam.transform.rotation;

            float maxDistance = 999f;

            RaycastHit hit;

            Vector2 centerScreen = new Vector2(Screen.width/2, Screen.height/2);
            Ray ray = cam.ScreenPointToRay(centerScreen);
            
            if(Physics.Raycast(ray, out hit, maxDistance, aimColliderMask))
            {
                if (hit.collider.gameObject.name == "Enemy")
                {
                    noEnemyCrossHair.SetActive(false);
                    enemyCrossHair.SetActive(true);
                }

                else
                {
                    enemyCrossHair.SetActive(false);
                    noEnemyCrossHair.SetActive(true);
                }

                if (hit.collider.gameObject.name == "Enemy" && shotFired == true)
                {
                    enemyTakeDamage = true;
                }

                else
                {
                    enemyTakeDamage = false;
                }
            }
        }
       
        // To Stop player movement while attacking. This code might be deleted in the final game
        if (lightAttack == true)
        {
            timer += Time.deltaTime;

            secondCombo = buttonPressed > 1;

            if (secondCombo == true)
            {
                activateSecondCombo = true;

                if (timer > secondAttackAnimationTimer)
                {
                    activateSecondCombo = false;
                    buttonPressed = 0;
                    timer = 0;
                }
            }

            else if (activateSecondCombo == false)
            {
                if (timer > animationTimer)
                {
                    timer = 0;
                    buttonPressed = 0;
                    activateSecondCombo = false;
                    lightAttack = false;
                }
            }
        }

        else /*(lightAttack == false)*/
        {
            direction = playerRigidbody.velocity = moveDir * directionSpeed;

            // if player is moving, it will have the same rotation as the camera and will help change the direction.
            if (moveDir != Vector3.zero)
            {
                transform.rotation = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0);
                cam.transform.localEulerAngles = Vector3.zero;
            }
        }
    }

    public bool EnemyDamage()
    {
        return enemyTakeDamage;
    }

    public bool SecondComboDamage()
    {
        return secondCombo;
    }

}




