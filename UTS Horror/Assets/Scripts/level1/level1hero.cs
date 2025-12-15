using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class level1hero : MonoBehaviour
{
    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 2f;
    public float minY = -60f;
    public float maxY = 60f;

    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;

    [Header("References")]
    public Transform cam;

    // Variables Internal
    private Rigidbody rb;
    private Animator animator;
    private float rotationX = 0f;
    private float rotationY = 0f;

    // Input Variables
    private float inputH, inputV;
    private bool isSprinting;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (animator != null)
        {
            animator.applyRootMotion = false;
        }

        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;                  

        rotationX = transform.eulerAngles.y;
        if (cam != null)
        {
            rotationY = cam.localEulerAngles.x;
        }
    }

    void Update()
    {
        // jika pause, gabisa ngapa ngapain
        if (Time.timeScale == 0f) return;

        HandleMouseLook();
        HandleInput();
        HandleAnimation();
    }

    void FixedUpdate()
    {
        if (Time.timeScale == 0f) return;
        HandleMovementPhysics();
    }

    void HandleMouseLook()
    {
        // Kamera hanya berputar jika KLIK KANAN ditahan
        if (Input.GetMouseButton(1))
        {
            // Saat klik kanan ditahan, kita sembunyikan kursor sementara (opsional, biar enak)
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            rotationX += mouseX;
            rotationY -= mouseY;
            rotationY = Mathf.Clamp(rotationY, minY, maxY);

            Quaternion targetRotation = Quaternion.Euler(0, rotationX, 0);
            rb.MoveRotation(targetRotation);

            if (cam != null)
            {
                cam.localRotation = Quaternion.Euler(rotationY, 0, 0);
            }
        }
        else
        {
            // Jika klik kanan dilepas, kursor kembali bebas
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    void HandleInput()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");

        bool hasMovementInput = (Mathf.Abs(inputH) > 0.1f || Mathf.Abs(inputV) > 0.1f);
        isSprinting = hasMovementInput && Input.GetKey(KeyCode.LeftShift);
    }

    void HandleAnimation()
    {
        if (animator == null) return;

        bool isWalking = (Mathf.Abs(inputH) > 0.1f || Mathf.Abs(inputV) > 0.1f);

        animator.SetBool("isWalk", isWalking);
        animator.SetBool("isRun", isSprinting);
    }

    void HandleMovementPhysics()
    {
        float targetSpeed = isSprinting ? runSpeed : walkSpeed;

        Vector3 moveDirection = (transform.right * inputH + transform.forward * inputV).normalized;
        Vector3 targetVelocity = moveDirection * targetSpeed;

        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }

    void OnDestroy()
    {
        // Pastikan kursor tetap ada saat objek hancur
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}