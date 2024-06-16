using System;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject inputManager;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject destination;

    private GameInput input;

    
    private bool playerAttacking= false;
 

    private void Awake()
    {
        input= inputManager.GetComponent<GameInput>();
    }

    private void Start()
    {
        input.OnLightAttackPerformed += PlayerAttack;
        input.OnLightAttackCanceled += CancelAttack;
    
    }

    // This is to see if player is attacking so enemy can shield
    private void PlayerAttack(object receiver, EventArgs e)
    {
        playerAttacking = true;
        
    }

    // This is to see if player is attacking so enemy can shield
    private void CancelAttack(object receiver, EventArgs e)
    {
        float delayTimer = 1f;
        Invoke("DelayCancel", delayTimer);

    }
    private void DelayCancel()
    {
        playerAttacking = false;
    }

    public bool PlayerAttacking()
    {
        return playerAttacking;
    }
}
