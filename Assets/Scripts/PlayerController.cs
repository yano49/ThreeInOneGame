using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Private Variables
    private float speed = 20.0f;
    private float turnSpeed = 45.0f;
    private float horizontalInput;
    private float forwardInput;

    // Move action with WASD / arrow key bindings (set in the Inspector)
    public InputAction moveAction;

    // Start is called before the first frame update
    void Start()
    {
        moveAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        // Read the player's input
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        horizontalInput = moveInput.x;
        forwardInput = moveInput.y;

        // Move the vehicle forward based on vertical input
        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
        // Rotate the vehicle based on horizontal input
        transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime);
    }

    void OnDisable()
    {
        moveAction.Disable();
    }
}
