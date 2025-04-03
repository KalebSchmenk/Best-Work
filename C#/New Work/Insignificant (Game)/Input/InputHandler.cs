using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// DESIGN PATTERN: Front Controller Pattern.
/// All player inputs pass through here.
/// </summary>
public class InputHandler : MonoBehaviour
{
    // Why so many delegate types that have the same signature?
    // Because it is possible that they may change in the future.
    // Instead of having to make a specific new one when we want the signature to change we just change its own signature.
    // Lots of copy-paste code, yes, but way more verbose about what its doing rather than a generic version.

    public delegate void MoveInputRecieved(Vector2 dir);
    public MoveInputRecieved OnMoveInputRecieved;

    public delegate void JumpInputRecieved();
    public JumpInputRecieved OnJumpInputRecieved;

    public delegate void LookInputRecieved(Vector2 dir);
    public LookInputRecieved OnLookInputRecieved;

    public delegate void FireInputRecieved();
    public FireInputRecieved OnFireInputRecieved;

    public delegate void InteractInputRecieved();
    public InteractInputRecieved OnInteractInputRecieved;

    public delegate void CrouchInputRecieved();
    public CrouchInputRecieved OnCrouchInputRecieved;

    public delegate void SprintInputRecieved();
    public SprintInputRecieved OnSprintInputRecieved;

    public delegate void RollInputRecieved();
    public RollInputRecieved OnRollInputRecieved;

    public delegate void AimInputRecieved();
    public AimInputRecieved OnAimInputRecieved;

    public PlayerInput playerInput;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        OnMoveInputRecieved?.Invoke(callbackContext.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            OnJumpInputRecieved?.Invoke();
        }
    }

    public void OnInteract(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            OnInteractInputRecieved?.Invoke();
        }
    }

    public void OnLook(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            OnLookInputRecieved?.Invoke(callbackContext.ReadValue<Vector2>());
        }
    }

    public void OnFire(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            OnFireInputRecieved?.Invoke();
        }
    }

    private void Update()
    {
        if (playerInput.actions["Fire"].IsPressed())
        {
            OnFireInputRecieved?.Invoke();
        }
    }

    public void OnCrouch(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            OnCrouchInputRecieved?.Invoke();
        }
    }

    public void OnSprint(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            OnSprintInputRecieved?.Invoke();
        }
    }

    public void OnRoll(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            OnRollInputRecieved?.Invoke();
        }
    }

    public void OnAim(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            OnAimInputRecieved?.Invoke();
        }
    }
}
