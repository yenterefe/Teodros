using System;
using UnityEngine;
using UnityEngine.InputSystem;  


public class GameInput : MonoBehaviour
{
    [SerializeField] GameObject player;
    private PlayerAnimation playerAnimation;
    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private Vector2 cameraInput;
    private int weaponSwitchButtonPressed = 0;
    public EventHandler OnReady;
    public EventHandler OnLightAttackPerformed; 
    public EventHandler OnLightAttackCanceled;
    public EventHandler OnSwitchToSwordPerformed; 
    public EventHandler OnSwitchToRiflePerformed;
    public EventHandler OnRifleAimPerformed; 
    public EventHandler OnRifleAimCanceled; 
    public EventHandler OnRifleFirePerformed; 
    public EventHandler OnRifleFireCanceled;
    public EventHandler OnShieldPerformed;
    public EventHandler OnShieldCanceled;

    private void Awake()
    {
        playerAnimation = player.GetComponent<PlayerAnimation>();

        inputActions = new PlayerInputActions();

        inputActions.Player.Enable();

        inputActions.Camera.Rotation.Enable();

        inputActions.Camera.Rotation.performed += Rotation_performed;

        inputActions.Camera.Rotation.canceled += Rotation_canceled;

        inputActions.Player.Movement.performed += Movement_performed;

        inputActions.Player.Movement.canceled += Movement_canceled;

        inputActions.Player.LightAttack.performed += LightAttack_performed;

        inputActions.Player.LightAttack.canceled += LightAttack_canceled;

        inputActions.Player.SwitchToSword.performed += SwitchToSword_performed;

        inputActions.Player.SwitchtoRifle.performed += SwitchtoRifle_performed;

        inputActions.Player.RifleAim.performed += RifleAim_performed;

        inputActions.Player.RifleAim.canceled += RifleAim_canceled;

        inputActions.Player.FireRifle.performed += FireRifle_performed;

        inputActions.Player.FireRifle.canceled += FireRifle_canceled;

        inputActions.Player.Shield.performed += Shield_performed;

        inputActions.Player.Shield.canceled += Shield_canceled;

    }

    private void Shield_canceled(InputAction.CallbackContext obj)
    {
       if(OnShieldCanceled != null)
        {
            OnShieldCanceled(this, EventArgs.Empty);
        }
    }

    private void Shield_performed(InputAction.CallbackContext obj)
    {
        if(OnShieldPerformed != null)
        {
            OnShieldPerformed(this, EventArgs.Empty);
        }
    }

    private void LightAttack_canceled(InputAction.CallbackContext obj)
    {
        if(OnLightAttackCanceled != null)
        {
            OnLightAttackCanceled(this, EventArgs.Empty);
        }
    }

    private void FireRifle_canceled(InputAction.CallbackContext obj)
    {
        if (OnRifleFireCanceled != null)
        {
            OnRifleFireCanceled(this, EventArgs.Empty);
        }
    }

    private void FireRifle_performed(InputAction.CallbackContext obj)
    {

        if(OnRifleFirePerformed != null)
        {
            OnRifleFirePerformed(this, EventArgs.Empty);
        }
    }

    private void RifleAim_canceled(InputAction.CallbackContext obj)
    {
        if(OnRifleAimCanceled != null)
        {
            OnRifleAimCanceled(this, EventArgs.Empty);  
        }
    }

    private void RifleAim_performed(InputAction.CallbackContext obj)
    {
        if(OnRifleAimPerformed != null)
        {
            OnRifleAimPerformed(this, EventArgs.Empty);
        }
    }

    private void SwitchtoRifle_performed(InputAction.CallbackContext obj)
    {
        if (OnSwitchToRiflePerformed != null)
        {
            OnSwitchToRiflePerformed(this, EventArgs.Empty);
        }
    }

    private void SwitchToSword_performed(InputAction.CallbackContext obj)
    {
        if(OnSwitchToSwordPerformed != null)
        {
            OnSwitchToSwordPerformed(this,EventArgs.Empty);
        }
    }

    private void LightAttack_performed(InputAction.CallbackContext obj)
    {
        if (OnLightAttackPerformed != null)
        {
            OnLightAttackPerformed(this, EventArgs.Empty);
        }
    }

    private void Rotation_canceled(InputAction.CallbackContext obj)
    {
        cameraInput = Vector2.zero;
    }

    private void Rotation_performed(InputAction.CallbackContext obj)
    {
        cameraInput= obj.ReadValue<Vector2>();
    }

    private void Movement_canceled(InputAction.CallbackContext obj)
    {
  
        moveInput = Vector2.zero;
    }

    private void Movement_performed(InputAction.CallbackContext obj)
    {
        moveInput = obj.ReadValue<Vector2>();
        moveInput = moveInput.normalized;
    }

    public Vector2 Player2DirectionNormalized()
    {
        return moveInput;
    }

    public Vector3 CameraDirectionNormalized()
    {
        return cameraInput;
    }

    public int ButtonPressed()
    {
        return weaponSwitchButtonPressed;
    }
}









