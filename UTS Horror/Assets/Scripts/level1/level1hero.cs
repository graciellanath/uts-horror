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
    public Transform cam; // Drag Main Camera ke sini

    // Variables Internal
    private Rigidbody rb;
    private Animator animator; // Variabel untuk Animator
    private float rotationX = 0f;
    private float rotationY = 0f;
    private bool isLooking = false;

    // Input Variables
    private float inputH, inputV;
    private bool isSprinting;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Cari Animator di object ini atau di anak-anaknya (BodyGuard)
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        // Matikan Root Motion via script untuk memastikan rotasi aman
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
        HandleMouseLook();
        HandleInput();     // Baca Input
        HandleAnimation(); // Update Animasi berdasarkan Input
    }

    void FixedUpdate()
    {
        HandleMovementPhysics();
    }

    void HandleMouseLook()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isLooking = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isLooking = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (isLooking)
        {
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
    }

    void HandleInput()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");

        // Cek apakah ada pergerakan (WASD)
        bool hasMovementInput = (Mathf.Abs(inputH) > 0.1f || Mathf.Abs(inputV) > 0.1f);

        // Syarat Sprint: Harus ada gerakan DULU, baru cek Shift
        // Ini menjawab request: "jika tekan shift kiri saja, dia tidak terjadi apa apa"
        isSprinting = hasMovementInput && Input.GetKey(KeyCode.LeftShift);
    }

    void HandleAnimation()
    {
        if (animator == null) return;

        // Cek apakah player sedang bergerak (Nilai absolut > 0)
        bool isWalking = (Mathf.Abs(inputH) > 0.1f || Mathf.Abs(inputV) > 0.1f);

        // Kirim ke Animator Controller
        // Logic: 
        // 1. isWalk true jika ada input WASD
        // 2. isRun true jika isWalk true DAN Shift ditekan
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
}