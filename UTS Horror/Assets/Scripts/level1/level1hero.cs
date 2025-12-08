using UnityEngine;

// Memaksa Unity menambahkan komponen wajib
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class level1hero : MonoBehaviour
{
    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 2f;
    public float minY = -60f;
    public float maxY = 60f;

    [Header("Movement Settings")]
    public float walkSpeed = 2f; // Disamakan dengan script playerfps
    public float runSpeed = 4f;

    // Reference
    public Transform cam; // Drag Main Camera ke sini

    // Variables Internal
    private Rigidbody rb;
    private float rotationX = 0f;
    private float rotationY = 0f;
    private bool isLooking = false;

    // Input Variables
    private float inputH, inputV;
    private bool isSprinting;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Kunci rotasi fisika agar karakter tidak jatuh terguling
        rb.freezeRotation = true;

        // Pastikan kursor muncul di awal
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Samakan rotasi awal script
        rotationX = transform.eulerAngles.y;
        if (cam != null)
        {
            rotationY = cam.localEulerAngles.x;
        }
    }

    void Update()
    {
        HandleMouseLook();
        HandleInput();
    }

    void FixedUpdate()
    {
        HandleMovementPhysics();
    }

    void HandleMouseLook()
    {
        // 1. Logic Klik Kanan (Tahan untuk putar)
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

        // 2. Eksekusi Rotasi
        if (isLooking)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            rotationX += mouseX;
            rotationY -= mouseY;
            rotationY = Mathf.Clamp(rotationY, minY, maxY);

            // Putar BADAN (Kiri-Kanan) menggunakan Rigidbody Rotation (Lebih aman untuk fisika)
            Quaternion targetRotation = Quaternion.Euler(0, rotationX, 0);
            rb.MoveRotation(targetRotation);

            // Putar KAMERA (Atas-Bawah)
            if (cam != null)
            {
                cam.localRotation = Quaternion.Euler(rotationY, 0, 0);
            }
        }
    }

    void HandleInput()
    {
        // Menggunakan GetAxisRaw agar gerakan responsif (langsung berhenti, tidak licin)
        inputH = Input.GetAxisRaw("Horizontal"); // A/D
        inputV = Input.GetAxisRaw("Vertical");   // W/S

        // Cek input sprint
        bool hasInput = (Mathf.Abs(inputH) > 0.1f || Mathf.Abs(inputV) > 0.1f);
        isSprinting = hasInput && Input.GetKey(KeyCode.LeftShift);
    }

    void HandleMovementPhysics()
    {
        // 1. Tentukan Speed
        float targetSpeed = isSprinting ? runSpeed : walkSpeed;

        // 2. Hitung Arah Gerak (Lokal ke Global)
        Vector3 moveDirection = (transform.right * inputH + transform.forward * inputV).normalized;

        // 3. Terapkan ke Velocity Rigidbody
        // Kita timpa velocity X dan Z dengan kecepatan target (ini membuat gerakan snappy/tidak licin)
        // Tapi kita biarkan velocity Y (gravitasi) apa adanya.
        Vector3 targetVelocity = moveDirection * targetSpeed;

        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }
}