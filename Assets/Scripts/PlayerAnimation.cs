using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private GameObject playerOBJ;

    [SerializeField] private GameObject InputManager;

    [SerializeField] private GameObject enemySword;

    [SerializeField] private float animationTransition = .15f;

    [SerializeField] GameObject aimCamera;
    [SerializeField] GameObject shakeCamera;

    [SerializeField] private GameObject sheatedSword;
    [SerializeField] private GameObject sword;

    [SerializeField] private GameObject shoulderRifle;
    [SerializeField] private GameObject rifle;

    [SerializeField] private float activateSheatedSwordTimer = 1f;
    [SerializeField] private float activateSwordTimer = 1.5f;

    [SerializeField] private float delayActiveCamera = 1f;

    [SerializeField] private ParticleSystem gunSmoke;

    [SerializeField] GameObject staminaBar;

    [SerializeField] GameObject ammoIndicator; 

    [SerializeField] private GameObject manager;

    private GameManager gameManager;

    private GameInput input;
    private Player player;
    private Animator playerAnim;
    private EnemySword enemySwordAttack;

    private bool isRifleAttackActive = false;
    private bool startTimer = false;
    private bool resetTimer = false;
    private bool isPlayerIdle = false;
    private bool isRifleAiming = false;
    private bool isRifleShot = false;
    private bool isShieldActive =false;
    private bool isLightAttackActive= false;
    private bool isLightComboAttackActive = false;
    private bool isStaminaDepleted = false;
    private bool isPlayerRunning = false;
    private bool isRifleModeActivated = false;


    private int moveXID;
    private int moveZID; 
    private int lightAttackAnimation1;
    private int lightAttackAnimation2;
    private int block;
    //private int rifleAim;
    //private int fireRifle;
    private int swordMovementAnimation;
    private int sheatingSword;
    private int swordButtonPressed = 0;
    private int riffleButtonPressed = 0;
    private int playerHitAnimation;

    private int totalAmmunition;

    private float timer = 0;
    private float blockSpeed = 0;

    private const string ATTACK_SWORD_MOVEMENT = "setSwordAttackMovement";
    private const string UNARMED_MOVEMENT = "unArmedMovement";
    private const string TUCK_SWORD = "sheathSword";
    private const string TUCK_RIFLE = "tuckRifle";
    private const string ATTACK_RIFLE_MOVEMENT = "setRifleAttackMovement";
    private const string AIM_RIFLE = "setAiming";
    private const string SHOOT= "shoot";
    private const string BLOCK = "block";
    private const string RUNNING = "Running";

    private void Awake()
    {
        player = playerOBJ.GetComponent<Player>();
        input = InputManager.GetComponent<GameInput>();
        playerAnim = GetComponent<Animator>();
        enemySwordAttack= enemySword.GetComponent<EnemySword>();
        gameManager = manager.GetComponent<GameManager>();  

        moveXID = Animator.StringToHash("MoveX");
        moveZID = Animator.StringToHash("MoveZ");

        lightAttackAnimation1 = Animator.StringToHash("light attack 1");
        lightAttackAnimation2 = Animator.StringToHash("light attack 2");

        swordMovementAnimation = Animator.StringToHash("Sword player movement");

        sheatingSword = Animator.StringToHash("Sheathing Sword");

        //rifleAim = Animator.StringToHash("Aiming Rifle");
        //fireRifle = Animator.StringToHash("Firing Rifle (1)");

        block = Animator.StringToHash("Block");
    }

    private void Start()
    {
        input.OnLightAttackPerformed += LightAttack;

        input.OnLightAttackCanceled += LightAttackCanceled;

        input.OnSwitchToSwordPerformed += SwordModeActivated;

        input.OnSwitchToRiflePerformed += RifleModeActivated;

        input.OnRifleAimPerformed += AimRifle;

        input.OnRifleFirePerformed += ShootRifle;

        input.OnRifleFireCanceled += StopShooting;

        input.OnRifleAimCanceled += CancelAim;

        input.OnShieldPerformed += BlockAttack;

        input.OnShieldCanceled += CancelBlock;

        input.OnPlayerRunningPeformed += PlayerRunning;

        input.OnPlayerRunningCanceled += PlayerWalking;
    }
    private void PlayerRunning(object receiver, EventArgs e)
    {
        isPlayerRunning = true;
    }

    private void PlayerWalking(object receiver, EventArgs e)
    {
        isPlayerRunning = false;
    }

    private void LightAttack(object receiver, EventArgs e)
    {
        startTimer = false;

        if (!isStaminaDepleted && !isRifleAttackActive)
        {
            isLightAttackActive = true;
            rifle.SetActive(false);
            shoulderRifle.SetActive(true);

            sheatedSword.SetActive(false);

            sword.SetActive(true);

            playerAnim.SetBool(UNARMED_MOVEMENT, false);

            playerAnim.SetBool(ATTACK_RIFLE_MOVEMENT, false);

            playerAnim.SetBool(ATTACK_SWORD_MOVEMENT, true);

            playerAnim.CrossFade(lightAttackAnimation1, animationTransition);

            if (player.SecondCombo())
            {
                isLightComboAttackActive = true;
                playerAnim.CrossFade(lightAttackAnimation2, animationTransition);
                timer = 0;
                resetTimer = true;
            }
        }
    }

    private void LightAttackCanceled(object receiver, EventArgs e)
    {
        startTimer = true;
    }

    private void CancelAim(object receiver, EventArgs e)
    {
        aimCamera.SetActive(false);
        ammoIndicator.SetActive(false);
        isRifleAiming = false;
        playerAnim.SetBool(AIM_RIFLE, false);

        if(rifle.activeInHierarchy)
        {
            startTimer = true;
        }
    }

    private void ShootRifle(object receiver, EventArgs e)
    {
        if (isRifleAiming && totalAmmunition > 0)
        {
            gunSmoke.Play();
            isRifleShot = true;
            playerAnim.SetBool(SHOOT, true);
            shakeCamera.SetActive(true);
        }
    }

    private void StopShooting(object receiver, EventArgs e)
    {
        playerAnim.SetBool(SHOOT, false);
        Invoke("DelayAimCamera", delayActiveCamera);
        isRifleShot = false;
    }

    private void BlockAttack(object receiver, EventArgs e)
    {

        isRifleModeActivated = false;

        isShieldActive = true;

        sword.SetActive(true);
        sheatedSword.SetActive(false);

        playerAnim.CrossFade(block, animationTransition);

        // if player has rifle and needs to block, it will automatically activate sword so the ammmunition indicator will be deactivated 
        if (ammoIndicator.activeInHierarchy == true)
        {
            ammoIndicator.SetActive(false);
        }

        if (rifle.activeInHierarchy == true)
        { 
            rifle.SetActive(false);
            shoulderRifle.SetActive(true);
        }

        blockSpeed += Time.deltaTime;
        //Debug.Log($"Block speed is {blockSpeed}");
    }

    private void CancelBlock(object receiver, EventArgs e)
    {
        blockSpeed = 0;
        isShieldActive=false;
        playerAnim.CrossFade(swordMovementAnimation, animationTransition);
        sword.SetActive(true);
    }

    private void RifleModeActivated(object receiver, EventArgs e)
    {

        isRifleModeActivated = true;

        if (!rifle.activeInHierarchy)
        {
            if(sword.activeInHierarchy)
            {
                sword.SetActive(false); 
                sheatedSword.SetActive(true);
            }

            ammoIndicator.SetActive(true);
            shoulderRifle.SetActive(false);
            rifle.SetActive(true);
            sword.SetActive(false);
            sheatedSword.SetActive(true);
            startTimer = true;
            isRifleAttackActive = true;
            playerAnim.SetBool(UNARMED_MOVEMENT, false);
            playerAnim.SetBool(ATTACK_SWORD_MOVEMENT, false);
            playerAnim.SetBool(ATTACK_RIFLE_MOVEMENT, true);
        }

        else
        {
            isRifleModeActivated=false;
            ammoIndicator.SetActive(false);
            startTimer = false;
            rifle.SetActive(false);
            shoulderRifle.SetActive(true);
            playerAnim.SetBool(ATTACK_RIFLE_MOVEMENT, false);
            playerAnim.SetBool(UNARMED_MOVEMENT, true);
        }
    }

    private void AimRifle(object receiver, EventArgs e)
    {
        if (isRifleAttackActive)
        {
            startTimer = false;
            isRifleAiming = true;
            playerAnim.SetBool(AIM_RIFLE, true);
        }

        if (rifle.activeInHierarchy)
        {
            aimCamera.SetActive(true);
        }
    }

    private void SwordModeActivated(object receiver, EventArgs e)
    {
        isRifleModeActivated = false;

        if (!sword.activeInHierarchy )
        {
            if(rifle.activeInHierarchy)
            {
                rifle.SetActive(false);
                shoulderRifle.SetActive(true);
            }

            startTimer = true;
            isRifleAttackActive = false;
            playerAnim.SetBool(UNARMED_MOVEMENT, false);
            playerAnim.SetBool(ATTACK_RIFLE_MOVEMENT, false);
            playerAnim.SetBool(ATTACK_SWORD_MOVEMENT, true);

            sheatedSword.SetActive(false);
            sword.SetActive(true);
        }

        else
        {
            startTimer = false;
            sheatedSword.SetActive(true);
            sword.SetActive(false);
            playerAnim.SetBool(ATTACK_SWORD_MOVEMENT, false);
            playerAnim.SetBool(UNARMED_MOVEMENT, true);
        }
    }

    private void FixedUpdate()
    {
        // Checks for player's stamina
        Stamina();

        // Sets timer for idle animation to kick in
        IdleTimer();

        DeactivateAmmunition();

        totalAmmunition = gameManager.GetAmmunition();

        ManageRunAnimation(); 

        Debug.Log(timer);
    }

    private void DelayAimCamera()
    {
        shakeCamera.SetActive(false);
    }

    public bool IsPlayerAiming()
    {
        return isRifleAiming;
    }

    public bool IsRifleShot()
    {
        return isRifleShot;
    }

    public bool ShieldActive()
    {
        return isShieldActive;
    }

    public bool LightAttack()
    {
        return isLightAttackActive;
    }

    public bool LightComboAttack()
    {
        return isLightComboAttackActive;
    }

    public bool IsRifleModeActivated()
    {
        return isRifleModeActivated;
    }

    // This will supervise player's stamina based on the stamina bar in the scene
    private void Stamina()
    {
        if (staminaBar.GetComponent<Slider>().value < 10f)
        {
            isStaminaDepleted = true;
        }
        else
        {
            isStaminaDepleted = false;
        }
    }

    private void DeactivateAmmunition()
    {
        if(!rifle.activeInHierarchy)
        {
            ammoIndicator.SetActive(false);
        }
    }

    private void IdleTimer()
    {
        float idleTime = 20f;

        if (startTimer == true)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
        }
        // Dont delete!
        //Debug.Log(timer);

        Vector2 playerMovement = input.Player2DirectionNormalized();

        playerAnim.SetFloat(moveXID, player.SmoothDumpAnimation().x);
        playerAnim.SetFloat(moveZID, player.SmoothDumpAnimation().y);


        // idle animation after a certain time has passed
        if (isRifleAttackActive == true && timer > idleTime && playerMovement == Vector2.zero)
        {
            playerAnim.SetTrigger(TUCK_RIFLE);
            
            rifle.SetActive(false);
            shoulderRifle.SetActive(true);

            isRifleAttackActive = false;
            startTimer = false;
            timer = 0;
            playerAnim.SetBool(UNARMED_MOVEMENT, true);
            playerAnim.SetBool(ATTACK_RIFLE_MOVEMENT, false);
        }

        // idle animation after a certain time has passed
        if (timer > idleTime && playerMovement == Vector2.zero)
        {
            playerAnim.SetTrigger(TUCK_SWORD);

            sword.SetActive(false);
            sheatedSword.SetActive(true);

            startTimer = false;
            timer = 0;
            playerAnim.SetBool(UNARMED_MOVEMENT, true);
            playerAnim.SetBool(ATTACK_SWORD_MOVEMENT, false);
        }
    }
   
    private void ManageRunAnimation()
    {
        if (isPlayerRunning == true && isStaminaDepleted == false)
        {
            playerAnim.SetBool(RUNNING, true);
        }

        else if (isPlayerRunning == true && isStaminaDepleted == true)
        {
            playerAnim.SetBool(RUNNING, false);
        }

        else
        {
            playerAnim.SetBool(RUNNING, false);
        }
    }
}
