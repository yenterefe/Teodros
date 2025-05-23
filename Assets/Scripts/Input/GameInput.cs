using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameInput : MonoBehaviour
{
    [SerializeField] GameObject player;

    //private PlayerAnimation playerAnimation;

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
    public EventHandler OnPlayerRunningPeformed;
    public EventHandler OnPlayerRunningCanceled;
    public EventHandler OnPausePerformed;
    public EventHandler OnUnPausePerformed;
    public EventHandler OnUIActionPerformed;

    private void Awake()
    {

        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
        inputActions.Camera.Rotation.Enable();
    }

    private void OnEnable()
    {
        //playerAnimation = player.GetComponent<PlayerAnimation>();

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

        inputActions.Player.Run.performed += Run_performed;

        inputActions.Player.Run.canceled += Run_canceled;

        inputActions.Player.Pause.performed += Pause_performed;

        inputActions.UI.Movement.performed += Movement_performed1;

        inputActions.UI.Select.performed += Select_performed;

        inputActions.UI.UnPause.performed += UnPause_performed;
    }

    private void OnDisable()
    {
        inputActions.Camera.Rotation.performed -= Rotation_performed;

        inputActions.Camera.Rotation.canceled -= Rotation_canceled;

        inputActions.Player.Movement.performed -= Movement_performed;

        inputActions.Player.Movement.canceled -= Movement_canceled;

        inputActions.Player.LightAttack.performed -= LightAttack_performed;

        inputActions.Player.LightAttack.canceled -= LightAttack_canceled;

        inputActions.Player.SwitchToSword.performed -= SwitchToSword_performed;

        inputActions.Player.SwitchtoRifle.performed -= SwitchtoRifle_performed;

        inputActions.Player.RifleAim.performed -= RifleAim_performed;

        inputActions.Player.RifleAim.canceled -= RifleAim_canceled;

        inputActions.Player.FireRifle.performed -= FireRifle_performed;

        inputActions.Player.FireRifle.canceled -= FireRifle_canceled;

        inputActions.Player.Shield.performed -= Shield_performed;

        inputActions.Player.Shield.canceled -= Shield_canceled;

        inputActions.Player.Run.performed -= Run_performed;

        inputActions.Player.Run.canceled -= Run_canceled;

        inputActions.Player.Pause.performed -= Pause_performed;

        inputActions.UI.Movement.performed -= Movement_performed1;

        inputActions.UI.Select.performed -= Select_performed;

        inputActions.UI.UnPause.performed -= UnPause_performed;
    }

    private void UnPause_performed(InputAction.CallbackContext obj)
    {
        if(OnUnPausePerformed != null)
        {
            OnUnPausePerformed(this, EventArgs.Empty);
            inputActions.Player.Enable();
            inputActions.Camera.Rotation.Enable();
            inputActions.UI.Movement.Disable();
            inputActions.UI.Select.Disable();
            inputActions.UI.UnPause.Disable();
        }
    }

    private void Pause_performed(InputAction.CallbackContext obj)
    {
        if (OnPausePerformed != null)
        {
            OnPausePerformed(this, EventArgs.Empty);
            inputActions.Player.Disable();
            inputActions.Camera.Rotation.Disable();
            inputActions.UI.Movement.Enable();
            inputActions.UI.Select.Enable();
            inputActions.UI.UnPause.Enable();
        }
    }

    private void Select_performed(InputAction.CallbackContext obj)
    {
        if(OnUIActionPerformed != null)
        {
            OnUIActionPerformed(this, EventArgs.Empty); 
        }
    }

    private void Movement_performed1(InputAction.CallbackContext obj)
    {
       //Debug.Log("Movement performed");
    }

    private void Run_canceled(InputAction.CallbackContext obj)
    {
        if (OnPlayerRunningCanceled != null)
        {
            OnPlayerRunningCanceled(this, EventArgs.Empty);
        }
    }

    private void Run_performed(InputAction.CallbackContext obj)
    {

        if (OnPlayerRunningPeformed != null)
        {
            OnPlayerRunningPeformed(this, EventArgs.Empty);
        }
    }

    private void Shield_canceled(InputAction.CallbackContext obj)
    {
        if (OnShieldCanceled != null)
        {
            OnShieldCanceled(this, EventArgs.Empty);
        }
    }

    private void Shield_performed(InputAction.CallbackContext obj)
    {
        if (OnShieldPerformed != null)
        {
            OnShieldPerformed(this, EventArgs.Empty);
        }
    }

    private void LightAttack_canceled(InputAction.CallbackContext obj)
    {
        if (OnLightAttackCanceled != null)
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
        if (OnRifleFirePerformed != null)
        {
            OnRifleFirePerformed(this, EventArgs.Empty);
        }
    }

    private void RifleAim_canceled(InputAction.CallbackContext obj)
    {
        if (OnRifleAimCanceled != null)
        {
            OnRifleAimCanceled(this, EventArgs.Empty);
        }
    }

    private void RifleAim_performed(InputAction.CallbackContext obj)
    {
        if (OnRifleAimPerformed != null)
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
        if (OnSwitchToSwordPerformed != null)
        {
            OnSwitchToSwordPerformed(this, EventArgs.Empty);
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
        cameraInput = obj.ReadValue<Vector2>();
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