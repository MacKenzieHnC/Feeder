using UnityEngine;
using UnityEngine.InputSystem;

// IGameplayActions is an interface generated from the "gameplay" action map
// we added (note that if you called the action map differently, the name of
// the interface will be different). This was triggered by the "Generate Interfaces"
// checkbox.
public class PlayerController : MonoBehaviour, PlayerControls.IPlayerActions
{
    // MyPlayerControls is the C# class that Unity generated.
    // It encapsulates the data from the .inputactions asset we created
    // and automatically looks up all the maps and actions for us.
    PlayerControls controls;

    private Vector2 moveInput;
    private float moveSpeed = 50f;

    private Rigidbody rb;

    public void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        moveInput = Vector2.zero;
    }

    public void OnEnable()
    {
        if (controls == null)
        {
            controls = new PlayerControls();
            // Tell the "gameplay" action map that we want to get told about
            // when actions get triggered.
            controls.Player.SetCallbacks(this);
        }
        controls.Player.Enable();
    }

    public void OnDisable()
    {
        controls.Player.Disable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnCameraMovement(InputAction.CallbackContext context)
    {
    }

    public void OnJump(InputAction.CallbackContext context)
    {
    }

    private void FixedUpdate()
    {
        float facing = Camera.main.transform.eulerAngles.y;
        Vector3 force = Quaternion.Euler(0, facing, 0) * new Vector3(moveInput.x, 0, moveInput.y);
        force *= 20 * (moveSpeed - rb.velocity.magnitude);
        rb.AddForce(force);

        if (rb.velocity.magnitude > 0.2)
            rb.transform.forward = rb.velocity;
    }
}
