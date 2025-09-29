using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public Vector2 MovementInputVector { get; private set; }
    public InputActionAsset InputActions;

    public void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    public void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }

    public void OnMove(InputValue inputValue)
    {
        MovementInputVector = inputValue.Get<Vector2>();
    }

    public void OnConsume()
    {

    }

    public void OnPause()
    {

    }

    public void OnPoop()
    {

    }

    public void OnSprint()
    {

    }
}
