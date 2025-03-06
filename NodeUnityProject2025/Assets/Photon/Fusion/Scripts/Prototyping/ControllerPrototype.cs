using UnityEngine;
using Fusion;

[ScriptHelp(BackColor = EditorHeaderBackColor.Steel)]
public class ControllerPrototype : NetworkBehaviour {
    protected NetworkCharacterControllerPrototype _ncc;
    protected NetworkRigidbody _nrb;
    protected NetworkRigidbody2D _nrb2d;
    protected NetworkTransform _nt;

    [Networked]
    public Vector3 MovementDirection { get; set; }

    public float Speed = 6f;
    public float MouseSensitivity = 400f; // Increased sensitivity
    private float _rotationX = 0f;
    private bool isCursorLocked = true;

    public void Awake() {
        CacheComponents();
        LockCursor(true);
    }

    public override void Spawned() {
        CacheComponents();
    }

    private void CacheComponents() {
        if (!_ncc) _ncc = GetComponent<NetworkCharacterControllerPrototype>();
        if (!_nrb) _nrb = GetComponent<NetworkRigidbody>();
        if (!_nrb2d) _nrb2d = GetComponent<NetworkRigidbody2D>();
        if (!_nt) _nt = GetComponent<NetworkTransform>();
    }

    public override void FixedUpdateNetwork() {
        if (Runner.Config.PhysicsEngine == NetworkProjectConfig.PhysicsEngines.None) {
            return;
        }

        if (Object.HasInputAuthority) { 
            HandleMouseLook();
            HandleCursorLockToggle();
        }

        Vector3 moveDirection = Vector3.zero;

        if (GetInput(out NetworkInputPrototype input)) {
            if (input.IsDown(NetworkInputPrototype.BUTTON_FORWARD)) {
                moveDirection += transform.forward;  // Move forward relative to player rotation
            }
            if (input.IsDown(NetworkInputPrototype.BUTTON_BACKWARD)) {
                moveDirection -= transform.forward;  // Move backward relative to player rotation
            }
            if (input.IsDown(NetworkInputPrototype.BUTTON_LEFT)) {
                moveDirection -= transform.right;  // Move left relative to player rotation
            }
            if (input.IsDown(NetworkInputPrototype.BUTTON_RIGHT)) {
                moveDirection += transform.right;  // Move right relative to player rotation
            }

            moveDirection = moveDirection.normalized;
            MovementDirection = moveDirection;

            if (input.IsDown(NetworkInputPrototype.BUTTON_JUMP)) {
                if (_ncc) {
                    _ncc.Jump();
                } else {
                    moveDirection += Vector3.up;
                }
            }
        } else {
            moveDirection = MovementDirection;
        }

        if (_ncc) {
            _ncc.Move(moveDirection * Speed);
        } else if (_nrb && !_nrb.Rigidbody.isKinematic) {
            _nrb.Rigidbody.AddForce(moveDirection * Speed);
        } else if (_nrb2d && !_nrb2d.Rigidbody.isKinematic) {
            Vector2 direction2d = new Vector2(moveDirection.x, moveDirection.z);
            _nrb2d.Rigidbody.AddForce(direction2d * Speed);
        } else {
            transform.position += moveDirection * Speed * Runner.DeltaTime;
        }
    }

    private void HandleMouseLook() {
        if (!Object.HasInputAuthority || !isCursorLocked) return;

        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;

        _rotationX -= mouseY;
        _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);

        // Rotate only the camera up/down
        Camera.main.transform.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);
        
        // Rotate the player left/right
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleCursorLockToggle() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            isCursorLocked = !isCursorLocked;
            LockCursor(isCursorLocked);
        }
    }

    private void LockCursor(bool shouldLock) {
        Cursor.lockState = shouldLock ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !shouldLock;
    }
}
