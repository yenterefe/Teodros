using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private GameObject playerOBJ;
    [SerializeField] private GameObject InputManager;

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

    private GameInput input;
    private Player player;
    private Animator playerAnim;

    private bool rifleAttack = false;
    private bool startTimer = false;
    private bool resetTimer = false;
    private bool playerisIdle = false;
    private bool aimRifle = false;
    private bool rifleShot = false;


    private int moveXID;
    private int moveZID;
    private int lightAttackAnimation1;
    private int lightAttackAnimation2;
    private int rifleAim;
    private int fireRifle;
    private int swordMovementAnimation;
    private int sheatingSword;
    private float timer = 0;

    private const string _ATTACKSWORDMOVEMENT = "setSwordAttackMovement";
    private const string _UNARMEDMOVEMENT = "unArmedMovement";
    private const string _TUCKSWORD = "sheathSword";
    private const string _TUCKRIFLE = "tuckRifle";
    private const string _WITHDRAWSWORD = "withdrawSword";
    private const string _WITHDRAWRIFLE = "withdrawRifle";
    private const string _ATTACKRIFLEMOVEMENT = "setRifleAttackMovement";
    private const string _AIMRIFLE = "setAiming";
    private const string _SHOOT= "shoot";

    private void Awake()
    {
        player = playerOBJ.GetComponent<Player>();
        input = InputManager.GetComponent<GameInput>();
        playerAnim = GetComponent<Animator>();

        moveXID = Animator.StringToHash("MoveX");
        moveZID = Animator.StringToHash("MoveZ");

        lightAttackAnimation1 = Animator.StringToHash("light attack 1");
        lightAttackAnimation2 = Animator.StringToHash("light attack 2");

        swordMovementAnimation = Animator.StringToHash("Sword player movement");

        sheatingSword = Animator.StringToHash("Sheathing Sword");

        rifleAim = Animator.StringToHash("Aiming Rifle");
        fireRifle = Animator.StringToHash("Firing Rifle (1)");

    }

    private void Start()
    {
        input.OnLightAttackPerformed += LightAttack;

        input.OnSwitchToSwordPerformed += SwordModeActivated;

        input.OnSwitchToRiflePerformed += RifleModeActivated;

        input.OnRifleAimPerformed += AimRifle;

        input.OnRifleFirePerformed += ShootRifle;

        input.OnRifleFireCanceled += StopShooting;

        input.OnRifleAimCanceled += CancelAim;
    }

    private void RifleModeActivated(object receiver, EventArgs e)
    {
        shoulderRifle.SetActive(false);
        rifle.SetActive(true);

        sword.SetActive(false);
        sheatedSword.SetActive(true);

        rifleAttack = true;
        playerAnim.SetTrigger(_WITHDRAWRIFLE);
        playerAnim.SetBool(_ATTACKSWORDMOVEMENT, false);
        playerAnim.SetBool(_ATTACKRIFLEMOVEMENT, true);
    }

    private void SwordModeActivated(object receiver, EventArgs e)
    {
        rifleAttack=false;
        playerAnim.SetTrigger(_WITHDRAWSWORD);
        playerAnim.SetBool(_ATTACKSWORDMOVEMENT, true);

        Invoke("DeactivateSheatingSword", activateSheatedSwordTimer);
        Invoke("ActivateSword", activateSwordTimer);
    }

    private void DeactivateSheatingSword()
    {
        sheatedSword.SetActive(false);
    }

    private void ActivateSword()
    {
        sword.SetActive(true);
        startTimer = true;
    }

    private void LightAttack(object receiver, EventArgs e)
    {
        {
            rifle.SetActive(false);
            shoulderRifle.SetActive(true);

            sheatedSword.SetActive(false);

            sword.SetActive(true);

            startTimer = true;

            playerAnim.SetBool(_ATTACKSWORDMOVEMENT, true);

            playerAnim.CrossFade(lightAttackAnimation1, animationTransition);

            if (player.SecondCombo() == true)
            {
                playerAnim.CrossFade(lightAttackAnimation2, animationTransition);
                timer = 0;
                resetTimer = true;
            }
        }
    }
   
    private void  AimRifle (object receiver, EventArgs e)
    {

        aimCamera.SetActive(true);

        if (rifleAttack==true)
        {
            aimRifle = true;
            startTimer = true;
            playerAnim.SetBool(_AIMRIFLE, true);
        }
    }

    private void CancelAim( object receiver, EventArgs e)
    {
        aimCamera.SetActive(false);

        aimRifle=false;
        playerAnim.SetBool(_AIMRIFLE, false);   
    }

    private void ShootRifle(object receiver, EventArgs e)
    {
        if (aimRifle == true)
        {
            rifleShot = true;
            playerAnim.SetBool(_SHOOT, true);
            shakeCamera.SetActive(true);
        }     
    }

    private void StopShooting(object receiver, EventArgs e)
    {       
        playerAnim.SetBool(_SHOOT, false);
        Invoke("DelayAimCamera", delayActiveCamera);
        rifleShot=false;
    }

    private void DelayAimCamera()
    {
        shakeCamera.SetActive(false);
    }

    private void Update()
    {
        float idleTime = 20f;

        if (startTimer==true)
        {
            timer += Time.deltaTime;
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

        if (timer> idleTime && playerMovement == Vector2.zero)
        {
            playerAnim.SetTrigger(_TUCKSWORD);

            sword.SetActive(false);
            sheatedSword.SetActive(true);

            playerisIdle = true;

            if(playerisIdle==true)
            {
                startTimer=false;
                timer = 0;
                playerAnim.SetBool(_UNARMEDMOVEMENT, true);
            }
        }
    }

    public bool IsPlayerAiming()
    {
        return aimRifle;
    }

    public bool IsRifleShot()
    {
        return rifleShot;
    }
}
