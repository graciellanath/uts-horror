using UnityEngine;

// Wajib ada Rigidbody supaya script jalan
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class level1hero : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f; // Kecepatan jalan
    public float sprintSpeed = 8f; // Kecepatan lari
    public float mouseSensitivity = 150f;

    [Header("References")]
    public Transform cam; // Drag Main Camera ke sini

    private Rigidbody rb;
    private float currentSpeed;
    private float xRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Pastikan rigidbody tidak guling-guling kena fisika
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Kita pisah: Update untuk Input & Rotasi
        MouseLook();
        InputMovement();
    }

    // FixedUpdate khusus untuk urusan Gerak Fisika (Rigidbody)
    void FixedUpdate()
    {
        MovePhysics();
    }

    // Variabel untuk menyimpan input sementara
    float inputH, inputV;

    void InputMovement()
    {
        inputH = Input.GetAxisRaw("Horizontal"); // A/D
        inputV = Input.GetAxisRaw("Vertical");   // W/S
        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
    }

    void MovePhysics()
    {
        // Hitung arah gerak berdasarkan arah hadap karakter saat ini
        // transform.right = arah kanan lokal, transform.forward = arah depan lokal
        Vector3 direction = (transform.right * inputH + transform.forward * inputV).normalized;

        // Masukkan ke velocity Rigidbody
        // Kita biarkan rb.velocity.y (sumbu Y) apa adanya supaya gravitasi tetap jalan
        rb.linearVelocity = new Vector3(direction.x * currentSpeed, rb.linearVelocity.y, direction.z * currentSpeed);
    }

    void MouseLook()
    {
        // Logika Kursor (Sama seperti sebelumnya)
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Input.GetMouseButtonUp(1))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Rotasi hanya saat klik kanan tahan
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Putar BADAN (Sumbu Y)
            // Menggunakan MoveRotation lebih aman untuk Rigidbody dibanding transform.Rotate
            Quaternion deltaRotation = Quaternion.Euler(Vector3.up * mouseX);
            rb.MoveRotation(rb.rotation * deltaRotation);

            // Putar KAMERA (Sumbu X / Dongak-Nunduk)
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -85f, 85f);

            if (cam != null)
            {
                cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            }
        }
    }
}