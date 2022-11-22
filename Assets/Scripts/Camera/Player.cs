using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class Player : NetworkBehaviour
{
    
    [SerializeField] float moveSpeed = 20f;
    Vector3 moveValue;
    Vector3 zoomValue;
    Vector2 rotationValue;
    Vector3 followOffset;
    float maxZoomOut = 15f;
    float maxZoomIn = -8f;
    float rotationSpeed = 80f;



    public override void OnStartAuthority()
    {
        
        base.OnStartAuthority();
        
        UnityEngine.InputSystem.PlayerInput playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        playerInput.enabled = true;
    }

    private void Update() 
    {
        HandleCameraMove();
        HandleCameraZoom();
        HandleCameraRotation();
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveValue = context.ReadValue<Vector3>() * Time.deltaTime * moveSpeed;
    }

    public void Zoom(InputAction.CallbackContext context)
    {
        zoomValue = context.ReadValue<Vector3>();
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        rotationValue = context.ReadValue<Vector2>();
    }

    void HandleCameraMove()
    {
        transform.Translate(moveValue);
    }

    void HandleCameraZoom()
    {
        transform.Translate(zoomValue * Time.deltaTime);
        transform.position = new Vector3(
            transform.position.x, 
            Mathf.Clamp(transform.position.y, maxZoomIn, maxZoomOut), 
            transform.position.z);
    }

    void HandleCameraRotation()
    {
        transform.Rotate(0, rotationValue.x * rotationSpeed * Time.deltaTime, 0);
    }

}
