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

    private bool rifleAttack = false;
    private bool startTimer = false;
    private bool resetTimer = false;
    private bool playerisIdle = false;
    private bool aimRifle = false;
    private bool rifleShot = false;
    private bool shieldActive =false;
    private bool lightAttack= false;
    private bool lightComboAttack = false;
    private bool staminaDepleted = false;


    private int moveXID;
    private int moveZID;
    private int lightAttackAnimation1;
    private int lightAttackAnimation2;
    private int block;
    private int rifleAim;
    private int fireRifle;
    private int swordMovementAnimation;
    private int sheatingSword;
    private int swordButtonPressed = 0;
    private int riffleButtonPressed = 0;

    private int totalAmmunition; 



    private float timer = 0;
    private float blockSpeed = 0;

    private const string _ATTACKSWORDMOVEMENT = "setSwordAttackMovement";
    private const string _UNARMEDMOVEMENT = "unArmedMovement";
    private const string _TUCKSWORD = "sheathSword";
    private const string _TUCKRIFLE = "tuckRifle";
    private const string _WITHDRAWSWORD = "withdrawSword";
    private const string _WITHDRAWRIFLE = "withdrawRifle";
    private const string _ATTACKRIFLEMOVEMENT = "setRifleAttackMovement";
    private const string _AIMRIFLE = "setAiming";
    private const string _SHOOT= "shoot";
    private const string _BLOCK = "block";

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

        rifleAim = Animator.StringToHash("Aiming Rifle");
        fireRifle = Animator.StringToHash("Firing Rifle (1)");

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
    }

    private void DeactivateSheatingSword()
    {
        sheatedSword.SetActive(false);
    }

    private void ActivateSword()
    {
        sword.SetActive(true);
        Invoke("StartTimer", 10f);
    }

    private void StartTimer()
    {
        startTimer = true;
    }

    private void LightAttack(object receiver, EventArgs e)
    {
        startTimer = false;

        if (staminaDepleted == false)
        {
            lightAttack = true;
            rifle.SetActive(false);
            shoulderRifle.SetActive(true);

            sheatedSword.SetActive(false);

            sword.SetActive(true);

            playerAnim.SetBool(_ATTACKSWORDMOVEMENT, true);

            playerAnim.CrossFade(lightAttackAnimation1, animationTransition);

            if (player.SecondCombo() == true)
            {
                lightComboAttack = true;
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

    private void AimRifle(object receiver, EventArgs e)
    {
        if (rifleAttack == true)
        {
            startTimer = false;
            aimRifle = true;
            playerAnim.SetBool(_AIMRIFLE, true);
        }

        if(rifle.activeInHierarchy == true)
        {
            aimCamera.SetActive(true);
        }
    }

    private void CancelAim(object receiver, EventArgs e)
    {
        aimCamera.SetActive(false);
        ammoIndicator.SetActive(false);
        aimRifle = false;
        playerAnim.SetBool(_AIMRIFLE, false);

        if(rifle.activeInHierarchy ==true)
        {
            startTimer = true;
        }
    }

    private void ShootRifle(object receiver, EventArgs e)
    {
        if (aimRifle == true && totalAmmunition > 0)
        {
            gunSmoke.Play();
            rifleShot = true;
            playerAnim.SetBool(_SHOOT, true);
            shakeCamera.SetActive(true);
        }
    }

    private void StopShooting(object receiver, EventArgs e)
    {
        
        playerAnim.SetBool(_SHOOT, false);
        Invoke("DelayAimCamera", delayActiveCamera);
        rifleShot = false;
    }

    private void BlockAttack(object receiver, EventArgs e)
    {
        shieldActive = true;
        playerAnim.CrossFade(block, animationTransition);
        blockSpeed+=Time.deltaTime;
        //Debug.Log($"Block speed is {blockSpeed}");
    }

    private void CancelBlock(object receiver, EventArgs e)
    {
        blockSpeed = 0;
        shieldActive=false;
        playerAnim.CrossFade(swordMovementAnimation, animationTransition);
        sword.SetActive(true);
    }

    private void RifleModeActivated(object receiver, EventArgs e)
    {
        if (rifle.activeInHierarchy== false)
        {
            ammoIndicator.SetActive(true);
            shoulderRifle.SetActive(false);
            rifle.SetActive(true);
            sword.SetActive(false);
            sheatedSword.SetActive(true);
            startTimer = true;
            rifleAttack = true;
            playerAnim.SetTrigger(_WITHDRAWRIFLE);
            playerAnim.SetBool(_UNARMEDMOVEMENT, false);
            playerAnim.SetBool(_ATTACKSWORDMOVEMENT, false);
            playerAnim.SetBool(_ATTACKRIFLEMOVEMENT, true);
        }

        else
        {
            ammoIndicator.SetActive(false);
            startTimer = false;
            playerAnim.SetTrigger(_TUCKRIFLE);
            playerAnim.ResetTrigger(_WITHDRAWSWORD);
            playerAnim.ResetTrigger(_WITHDRAWRIFLE);
            rifle.SetActive(false);
            shoulderRifle.SetActive(true);
            playerAnim.SetBool(_ATTACKRIFLEMOVEMENT, false);
            playerAnim.SetBool(_UNARMEDMOVEMENT, true);
        }
    }

    private void SwordModeActivated(object receiver, EventArgs e)
    {
        if(sword.activeInHierarchy ==false)
        {
            rifleAttack = false;
            playerAnim.SetTrigger(_WITHDRAWSWORD);
            playerAnim.SetBool(_UNARMEDMOVEMENT, false);
            playerAnim.SetBool(_ATTACKRIFLEMOVEMENT, false);
            playerAnim.SetBool(_ATTACKSWORDMOVEMENT, true);

            Invoke("DeactivateSheatingSword", activateSheatedSwordTimer);
            Invoke("ActivateSword", activateSwordTimer);
        }

        else
        {
            startTimer = false;
            playerAnim.SetTrigger(_TUCKSWORD);
            playerAnim.ResetTrigger(_WITHDRAWSWORD);
            playerAnim.ResetTrigger(_WITHDRAWRIFLE);
            CancelInvoke("DeactivateSheatingSword");
            CancelInvoke("ActivateSword");
            sword.SetActive(false);
            sheatedSword.SetActive(true);
            playerAnim.SetBool(_ATTACKSWORDMOVEMENT, false);
            playerAnim.SetBool(_UNARMEDMOVEMENT, true);
        }
    }

    private void Update()
    {
        // Checks for player's stamina
        Stamina();

        // Sets timer for idle animation to kick in
        IdleTimer();

        totalAmmunition = gameManager.GetAmmunition();

        Debug.Log(timer);
    }

    private void DelayAimCamera()
    {
        shakeCamera.SetActive(false);
    }

    public bool IsPlayerAiming()
    {
        return aimRifle;
    }

    public bool IsRifleShot()
    {
        return rifleShot;
    }

    public bool ShieldActive()
    {
        return shieldActive;
    }

    public bool LightAttack()
    {
        return lightAttack;
    }

    public bool LightComboAttack()
    {
        return lightComboAttack;
    }

    private void Stamina()
    {
        if (staminaBar.GetComponent<Slider>().value < 10f)
        {
            staminaDepleted = true;
        }

        else
        {
            staminaDepleted = false;
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

        if (rifleAttack == true && timer > idleTime && playerMovement == Vector2.zero)
        {
            playerAnim.SetTrigger(_TUCKRIFLE);
            rifle.SetActive(false);
            shoulderRifle.SetActive(true);

            playerisIdle = true;

            if (playerisIdle == true)
            {
                rifleAttack = false;
                startTimer = false;
                timer = 0;
                playerAnim.SetBool(_UNARMEDMOVEMENT, true);
            }
        }

        if (timer > idleTime && playerMovement == Vector2.zero)
        {
            playerAnim.SetTrigger(_TUCKSWORD);

            sword.SetActive(false);
            sheatedSword.SetActive(true);

            playerisIdle = true;

            if (playerisIdle == true)
            {
                startTimer = false;
                timer = 0;
                playerAnim.SetBool(_UNARMEDMOVEMENT, true);
            }
        }
    }
}
