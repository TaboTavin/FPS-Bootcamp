using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    
    public float rotationSpeed = 2f;

    private CharacterController characterController;

    public bool invertVerticalAxis = false;

    private float verticalRotation = 0f;
    private float verticalRotationLimit = 45f;

    // Variables de Posiciones y sus velocidades
    public float normalSpeed = 5f;
    public float crouchSpeed = 2.5f;
    public float proneSpeed = 1.5f;

    public float nomralCameraHeight = 0.8f;
    public float crouchingCameraHeight = 0.3f;
    public float proningCameraHeight = 0.1f;

    public bool isCrouching = false;
    public bool isProning = false;

    public UIManager uiManager;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        //Oculta el cursor y lo centra en pantalla.
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Movement
        float vertical;
        float horizontal;
        float moveSpeed;

        if(isProning)
        {
            vertical = Input.GetAxis("Vertical") * proneSpeed;
            horizontal = Input.GetAxis("Horizontal") * proneSpeed;
            moveSpeed = proneSpeed;
        }
        else if(isCrouching)
        {
            vertical = Input.GetAxis("Vertical") * crouchSpeed;
            horizontal = Input.GetAxis("Horizontal") * crouchSpeed;
            moveSpeed = crouchSpeed;
        }
        else
        {
            vertical = Input.GetAxis("Vertical") * normalSpeed;
            horizontal = Input.GetAxis("Horizontal") * normalSpeed;
            moveSpeed = normalSpeed;
        }

        Vector3 movement = transform.forward * vertical + transform.right * horizontal;
        characterController.SimpleMove(movement);

        NormalCrouchProningPosition();

        // Camera
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        if(invertVerticalAxis)
        {
            mouseY *= -1f;
        }

        // Aplicar rotacion vertical con limitacion
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);

        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        transform.Rotate(0, mouseX, 0);
    }

    public void NormalCrouchProningPosition()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!isCrouching && !isProning)
            {
                isCrouching = true;
                uiManager.UpdatePosition("Crouching");
                Camera.main.transform.localPosition = new Vector3(0, crouchingCameraHeight, 0);
            }
            else if (isCrouching)
            {
                isCrouching = false;
                isProning = true;
                uiManager.UpdatePosition("Proning");
                Camera.main.transform.localPosition = new Vector3(0, proningCameraHeight, 0);
            }
            else if (isProning)
            {
                isProning = false;
                uiManager.UpdatePosition("Normal");
                Camera.main.transform.localPosition = new Vector3(0, nomralCameraHeight, 0);
            }
        }
    }
}
