using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    //Initialization Variables for Camera Control
    [SerializeField] Transform playerTransform;
    [SerializeField] GameObject cameraRig;
    [SerializeField] float moveSpeed = 20f;
    [SerializeField] float screenBorderThickness = 5f;
    [SerializeField] Vector2 screenXLimit = Vector2.zero;
    [SerializeField] Vector2 screenZLimit = Vector2.zero;
    Vector2 previousInput;
    Controls controls;

    //TODO: Add method to rotate camera and zoom camera
    
    //Overriding OnStartAuthority to only have one camera per player and enabling the Input System for movement
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        cameraRig.SetActive(true);
        controls = new Controls();
        controls.Player.Move.performed += SetPreviousInput;
        controls.Player.Move.canceled += SetPreviousInput;
        controls.Enable();
    }

    [ClientCallback]
    private void Update() 
    {
        if(!isOwned || !Application.isFocused) { return; }
        UpdateCameraPosition();

    }

    //Telling the camera to move, using either the WASD keys or moving the mouse
    //to the screen edge while holding the 'T' key
    void UpdateCameraPosition()
    {
        
        Vector3 pos = playerTransform.position;
        Vector3 cursorMovement = Vector3.zero;
        Vector2 cursorPosition = Mouse.current.position.ReadValue();

        if(previousInput == Vector2.zero && Keyboard.current.tKey.isPressed)
        {
            if(cursorPosition.y >= Screen.height - screenBorderThickness)
            {
                cursorMovement.z += 1;
            }
            else if(cursorPosition.y <= screenBorderThickness)
            {
                cursorMovement.z -= 1;
            }
        
            if(cursorPosition.x >= Screen.width - screenBorderThickness)
            {
                cursorMovement.x += 1;
            }
            else if(cursorPosition.x <= screenBorderThickness)
            {
                cursorMovement.x -= 1;
            }

        pos += cursorMovement.normalized * moveSpeed * Time.deltaTime;
        }
        else
        {
            pos += new Vector3(previousInput.x, 0f, previousInput.y) *moveSpeed * Time.deltaTime;
        }
        pos.x = Mathf.Clamp(pos.x, screenXLimit.x, screenXLimit.y);
        pos.z = Mathf.Clamp(pos.z, screenZLimit.x, screenZLimit.y);

        playerTransform.position = pos;

    }

    //Referencing previousInput (the input that comes from keyboard/mouse) and storing it for callback
    void SetPreviousInput(InputAction.CallbackContext ctx)
    {
        previousInput = ctx.ReadValue<Vector2>();
    }
}