using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Moves the ship and wraps it around the screen
/// </summary>

public class PlayerMovement : MonoBehaviour
{
    private float speed = 6f;
    private float horizontalSreenLimit = 10f;
    private float verticalScreenLimit = 6f;
    private Vector2 movementVector;

    // These start and stop listening for movement from PlayerInputHandler
    private void OnEnable()
    {
        PlayerInputHandler.onMovement += MovementInput;
    }

    private void OnDisable()
    {
        PlayerInputHandler.onMovement -= MovementInput;
    } 

    private void MovementInput(Vector2 input)
    {
        movementVector = input;
    }

    private void Update()
    {
        transform.Translate(new Vector3(movementVector.x, movementVector.y, 0) * Time.deltaTime * speed);

        // Jumps to other side when going off screen
        if (transform.position.x > horizontalSreenLimit || transform.position.x <= -horizontalSreenLimit)
        {
            transform.position = new Vector3(transform.position.x * -1f, transform.position.y, 0);
        }

        if (transform.position.y > verticalScreenLimit || transform.position.y <= -verticalScreenLimit)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y * -1, 0);
        }
    }
} 