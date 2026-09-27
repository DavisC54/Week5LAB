using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Owns the Controlls asset and sends inputs through static events so nothing else needs to touch the input system
/// </summary>
public class PlayerInputHandler : MonoBehaviour, Controls.IPlayerActions
{
    // Other scripts add their methods to these to hear the input
   public static UnityAction<Vector2> onMovement = delegate { };
   public static UnityAction onFire = delegate { };
   public static UnityAction onRestart = delegate { };

   private Controls controls;

   private void OnEnable()
    {
        if (controls == null)
        {
            controls = new Controls();
            // Makes Controls call for inputs from below
            controls.Player.SetCallbacks(this);
        }

        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    // CallbackContext reads start, performed and cancel 
    public void OnMove(InputAction.CallbackContext context)
    {
        onMovement(context.ReadValue<Vector2>());
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onFire();
        }
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onRestart();
        }
    }
}
