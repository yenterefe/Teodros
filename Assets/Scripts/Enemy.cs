using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject inputManager;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject shield;


    private GameInput input;
    private Animator enemyAnimation;
    private int blockAnimation;
    private int idleAnimation;
    private float normalizedTransitionDuration = 0.15f;

    private const string _SHIELDBLOCK = "Shield block";
    private const string _IDLE = "Unarmed Idle";

    private void Awake()
    {
        input= inputManager.GetComponent<GameInput>();
        enemyAnimation=enemyPrefab.GetComponent<Animator>();
        blockAnimation = Animator.StringToHash(_SHIELDBLOCK);
        idleAnimation = Animator.StringToHash(_IDLE);
    }

    private void Start()
    {
        input.OnLightAttackPerformed += BlockAttacks;
        input.OnLightAttackCanceled += LowersShield;


    }



    private void BlockAttacks(object receiver, EventArgs e)
    {
        // Enemy takes no damage if they block once, wriet script here later 


        // Play block Animation 
        enemyAnimation.CrossFade(blockAnimation, normalizedTransitionDuration);
    }

    private void LowersShield(object receiver, EventArgs e)
    {
        // the cancel is to quick so I'm delaying it by the delay float
        float delayAnimation = 1.5f;

        Invoke("IdleAnimation", delayAnimation);
        
    }

    private void IdleAnimation()
    {
        enemyAnimation.CrossFade(idleAnimation, normalizedTransitionDuration);
    }

}
