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
    public float moveSpeed = 5f;
    public float sprintMultiplier = 2f;
    public float gravity = -9.81f;

    [Header("Health Settings")]
    public int maxHealth = 100;
    public int health = 100;
    public TextMeshProUGUI healthText;

    private float rotationX = 0f;
    private float rotationY = 0f;
    private Transform cameraTransform;
    private CharacterController controller;
    private Animator animator; // Variabel Animator
    private bool isLooking = false;

    private Vector3 velocity;

    void Start()
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
        controller = GetComponent<CharacterController>();

        // Cari Animator di dalam anak objek (Model 3D kamu)
        animator = GetComponentInChildren<Animator>();

        // Cek jaga-jaga kalau lupa pasang Animator
        if (animator == null)
        {
            Debug.LogWarning("Animator tidak ditemukan di Player atau Child-nya!");
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        UpdateHealthUI();
        rotationX = transform.eulerAngles.y;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        if (Time.timeScale == 0f) return;

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

            transform.rotation = Quaternion.Euler(0, rotationX, 0);
            cameraTransform.localRotation = Quaternion.Euler(rotationY, 0, 0);
        }
    }

    void HandleMovement()
    {
        if (Time.timeScale == 0f) return;

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Cek apakah player sedang bergerak (WASD ditekan)
        // Magnitude > 0.1f artinya ada tombol yang ditekan
        bool isMoving = Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f;

        // Cek apakah player sedang lari (Bergerak + Shift Kiri)
        bool isSprinting = isMoving && Input.GetKey(KeyCode.LeftShift);

        // --- UPDATE ANIMASI ---
        if (animator != null)
        {
            // Set parameter isWalk (true jika bergerak)
            animator.SetBool("isWalk", isMoving);

            // Set parameter isRun (true jika bergerak + shift)
            animator.SetBool("isRun", isSprinting);
        }

        float currentSpeed = moveSpeed;
        if (isSprinting)
            currentSpeed *= sprintMultiplier;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * currentSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

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

    public void Heal(int amount)
    {
        if (health >= maxHealth) return;
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
    }

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

    void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}