using UnityEngine;
using Fusion;

[ScriptHelp(BackColor = EditorHeaderBackColor.Steel)]
public class ControllerPrototype : Fusion.NetworkBehaviour {
    protected NetworkCharacterControllerPrototype _ncc;
    protected NetworkRigidbody _nrb;
    protected NetworkRigidbody2D _nrb2d;
    protected NetworkTransform _nt;

    [Networked]
    public Vector3 MovementDirection { get; set; }

    public bool TransformLocal = false;

    [DrawIf(nameof(ShowSpeed), Hide = true)]
    public float Speed = 6f;
    public float MouseSensitivity = 100f;

    private float _rotationX = 0f;

    bool ShowSpeed => this && !TryGetComponent<NetworkCharacterControllerPrototype>(out _);

    public void Awake() {
        CacheComponents();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void Spawned() {
        CacheComponents();
    }

    private void CacheComponents() {
        if (!_ncc) _ncc     = GetComponent<NetworkCharacterControllerPrototype>();
        if (!_nrb) _nrb     = GetComponent<NetworkRigidbody>();
        if (!_nrb2d) _nrb2d = GetComponent<NetworkRigidbody2D>();
        if (!_nt) _nt       = GetComponent<NetworkTransform>();
    }

    public override void FixedUpdateNetwork() {
        if (Runner.Config.PhysicsEngine == NetworkProjectConfig.PhysicsEngines.None) {
            return;
        }

        Vector3 direction = Vector3.zero;
        if (GetInput(out NetworkInputPrototype input)) {
            if (input.IsDown(NetworkInputPrototype.BUTTON_FORWARD)) {
                direction += transform.forward;
            }
            if (input.IsDown(NetworkInputPrototype.BUTTON_BACKWARD)) {
                direction -= transform.forward;
            }
            if (input.IsDown(NetworkInputPrototype.BUTTON_LEFT)) {
                direction -= transform.right;
            }
            if (input.IsDown(NetworkInputPrototype.BUTTON_RIGHT)) {
                direction += transform.right;
            }

            direction = direction.normalized;
            MovementDirection = direction;

            if (input.IsDown(NetworkInputPrototype.BUTTON_JUMP)) {
                if (_ncc) {
                    _ncc.Jump();
                } else {
                    direction += Vector3.up;
                }
            }
        } else {
            direction = MovementDirection;
        }

        if (_ncc) {
            _ncc.Move(direction * Speed);
        } else if (_nrb && !_nrb.Rigidbody.isKinematic) {
            _nrb.Rigidbody.AddForce(direction * Speed);
        } else if (_nrb2d && !_nrb2d.Rigidbody.isKinematic) {
            Vector2 direction2d = new Vector2(direction.x, direction.z);
            _nrb2d.Rigidbody.AddForce(direction2d * Speed);
        } else {
            transform.position += direction * Speed * Runner.DeltaTime;
        }
    }

    public  void Update() {
        HandleMouseLook();
    }

    private void HandleMouseLook() {
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;

        _rotationX -= mouseY;
        _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_rotationX, transform.localRotation.eulerAngles.y + mouseX, 0f);
    }
}