using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(CharacterController))]
public class playerfps : MonoBehaviour
{
    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 2f;
    public float minY = -60f;
    public float maxY = 60f;

    [Header("Movement Settings")]

    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float gravity = -9.81f;

    private float terminalVelocity = -20f;

    [Header("Health Settings")]
    public int maxHealth = 100;
    public int health = 100;
    public TextMeshProUGUI healthText;

    // Variables Internal
    private float rotationX = 0f;
    private float rotationY = 0f;
    private Transform cameraTransform;
    private CharacterController controller;
    private Animator animator;
    private bool isLooking = false;
    private Vector3 velocity; // Untuk menghitung gravitasi

    void Start()
    {
        // Mengambil komponen yang dibutuhkan
        cameraTransform = GetComponentInChildren<Camera>().transform;
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        if (animator == null) Debug.LogWarning("Animator tidak ditemukan!");

        // Membuka kunci kursor mouse di awal
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        UpdateHealthUI();

        // Samakan rotasi kamera dengan rotasi player saat mulai
        if (cameraTransform != null)
        {
            rotationX = transform.eulerAngles.y;
            rotationY = cameraTransform.localEulerAngles.x;
        }
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        if (Time.timeScale == 0f) return;

        // Tahan Klik Kanan untuk memutar kamera
        if (Input.GetMouseButtonDown(1))
        {
            isLooking = true;
            Cursor.lockState = CursorLockMode.Locked; // Mengunci kursor di tengah
            Cursor.visible = false;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isLooking = false;
            Cursor.lockState = CursorLockMode.None; // Melepas kursor
            Cursor.visible = true;
        }

        if (isLooking)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            rotationX += mouseX;
            rotationY -= mouseY;
            rotationY = Mathf.Clamp(rotationY, minY, maxY);

            // Memutar badan karakter (Kiri/Kanan)
            transform.rotation = Quaternion.Euler(0, rotationX, 0);

            // Memutar kamera (Atas/Bawah)
            cameraTransform.localRotation = Quaternion.Euler(rotationY, 0, 0);
        }
    }

    void HandleMovement()
    {
        if (Time.timeScale == 0f) return;

        // --- 1. HANDLE GRAVITASI (Tanah) ---
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Reset gravitasi agar menempel di tanah
        }

        // --- 2. INPUT PERGERAKAN ---
        // Pakai GetAxisRaw agar responsif (tidak licin)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // Cek apakah ada input gerakan
        bool hasInput = (Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f);

        // Cek lari (Input ada + Shift ditekan)
        bool isSprinting = hasInput && Input.GetKey(KeyCode.LeftShift);

        // --- 3. PILIH KECEPATAN ---
        float targetSpeed = isSprinting ? runSpeed : walkSpeed;

        // --- 4. ANIMASI ---
        if (animator != null)
        {
            animator.SetBool("isWalk", hasInput);
            animator.SetBool("isRun", isSprinting);
        }

        // --- 5. HITUNG GERAKAN ---
        // Hitung arah gerak horizontal (X & Z)
        Vector3 moveDirection = (transform.right * moveX + transform.forward * moveZ).normalized;

        // Hitung gravitasi (Y)
        velocity.y += gravity * Time.deltaTime;
        if (velocity.y < terminalVelocity) velocity.y = terminalVelocity;

        // --- 6. EKSEKUSI (Final Move) ---
        // Gabungkan Gerakan Horizontal + Vertikal menjadi satu Vektor
        // Ini adalah KUNCI agar tidak stuttering (hanya panggil Move 1x per frame)
        Vector3 finalVelocity = (moveDirection * targetSpeed) + velocity;

        controller.Move(finalVelocity * Time.deltaTime);
    }

    // Fungsi Pengurangan Darah
    public void TakeDamage(int amount)
    {
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();

        if (health <= 0)
        {
            Debug.Log("Player mati!");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("GameOver");
        }
    }

    // Fungsi Tambah Darah
    public void Heal(int amount)
    {
        if (health >= maxHealth) return;
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
    }

    // Update UI Teks
    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {health}%";
            if (health > 70) healthText.color = Color.green;
            else if (health > 30) healthText.color = Color.yellow;
            else healthText.color = Color.red;
        }
    }

    // Pembersihan saat script hancur/pindah scene
    void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}