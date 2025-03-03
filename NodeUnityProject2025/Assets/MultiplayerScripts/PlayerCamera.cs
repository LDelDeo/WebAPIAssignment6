using UnityEngine;
using Mirror;

public class PlayerCamera : NetworkBehaviour
{
    private float verticalRotation = 0f;
    public float sensitivity = 2.0f;

    void Start()
    {
        // Ensure this script only affects the local player’s camera
        if (!isLocalPlayer)
        {
            gameObject.SetActive(false); // Disable the camera for non-local players
            return;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        // Vertical rotation (Up/Down)
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);

        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }
}
