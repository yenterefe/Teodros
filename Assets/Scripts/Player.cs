using System;
using UnityEngine;
using UnityEngine.UI; 


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
    
    private float timer = 0;
    private float buttonPressed = 0;

    private bool shotFired= false;
    private bool enemyTakeDamage= false;
    private bool secondCombo;
    private bool lightAttack;
    private bool activateSecondCombo = false;
    private bool isAiming;
    private bool enemySighted = false;   

    private Ray ray;

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

    //Stamina
    [SerializeField] private GameObject staminaBar;
    
    private float stamina;

    private bool staminaDepleted = false;
    
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
        gameInput.OnPlayerRunningPeformed += PlayerRunning;
        gameInput.OnPlayerRunningCanceled += PlayerWalking;
    }

    private void Update()
    {
        //Debug.Log("Is shot fired " + shotFired);
        //Debug.Log("Name of object aimed at " + gameObjectName);

        ManageStamina();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }


    public Vector3 Movement()
    {
        return moveDir;
    }

    private void PlayerRunning(object recevier, EventArgs e)
    {
        if(staminaDepleted == false)
        {
            directionSpeed = 5;
        }

        else
        {
            directionSpeed = 2; 
        }
        
    }

    private void PlayerWalking(object recevier, EventArgs e)
    {
        directionSpeed = 2;
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
        isAiming = playerAnimation.IsPlayerAiming();

        {
            Vector2 inputDirection = gameInput.Player2DirectionNormalized();

            // To smoothen corner directions for movement
            currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, inputDirection, ref animationVelocity, animationSmoothTime);

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
            // Player takes camera rotation
            transform.rotation = cam.transform.rotation;

            float maxDistance = 50f;

            RaycastHit hit;

            Vector2 centerScreen = new Vector2(Screen.width/2, Screen.height/2);
            ray = cam.ScreenPointToRay(centerScreen);
            
            if(Physics.Raycast(ray, out hit, maxDistance, aimColliderMask))
            {
                Debug.DrawRay(transform.position, centerScreen, Color.red);
                if (hit.collider.gameObject.name == "Enemy")
                {
                    enemySighted = true; 
                    Debug.Log("Enemy onsite!");
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

        if (buttonPressed== 2)
        {
            float secondComboTimer = .65f;
            activateSecondCombo=true;
            buttonPressed=0;
            Invoke("DeactivateSecondCombo",secondComboTimer);
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

    private void DeactivateSecondCombo()
    {
        activateSecondCombo = false;
    }

    public bool EnemyDamage()
    {
        return enemyTakeDamage;
    }

    public bool SecondComboDamage()
    {
        return secondCombo;
    }

    public bool IsAiming()
    {
        return isAiming;
    }

    public bool IsEnemySighted()
    {
        return enemySighted;
    }

    private void ManageStamina()    
    {
        stamina = staminaBar.GetComponent<Slider>().value;

        if(stamina <= 10f)
        {
            staminaDepleted = true;
        }

        else
        {
            staminaDepleted = false;
        }
    }
}




