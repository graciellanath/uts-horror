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

    // Internal Variables
    private float rotationX = 0f;
    private float rotationY = 0f;
    private Transform cameraTransform;
    private CharacterController controller;
    private Animator animator;
    private bool isLooking = false;
    private Vector3 velocity;

    void Start()
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        if (animator == null) Debug.LogWarning("Animator tidak ditemukan di anak objek!");

        // ilangin cursor di awal
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        UpdateHealthUI();

        if (cameraTransform != null)
        {
            rotationX = transform.eulerAngles.y;
            rotationY = cameraTransform.localEulerAngles.x;
        }
    }

    void Update()
    {
        // pause gabisa ngapa ngapain
        if (Time.timeScale == 0f) return;

        // lock cursor
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    {

        if (Input.GetMouseButtonDown(1))
        {
            isLooking = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isLooking = false;
        }

        if (isLooking)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            rotationX += mouseX;
            rotationY -= mouseY;
            rotationY = Mathf.Clamp(rotationY, minY, maxY);

            transform.rotation = Quaternion.Euler(0, rotationX, 0);
            if (cameraTransform != null)
                cameraTransform.localRotation = Quaternion.Euler(rotationY, 0, 0);
        }
    }

    void HandleMovement()
    {
        // Reset gravitasi saat di tanah
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        bool hasInput = (Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f);
        bool isSprinting = hasInput && Input.GetKey(KeyCode.LeftShift);

        float targetSpeed = isSprinting ? runSpeed : walkSpeed;

        // Update Animasi
        if (animator != null)
        {
            animator.SetBool("isWalk", hasInput);
            animator.SetBool("isRun", isSprinting);
        }

        // Kalkulasi Gerak
        Vector3 moveDirection = (transform.right * moveX + transform.forward * moveZ).normalized;

        // Kalkulasi Gravitasi
        velocity.y += gravity * Time.deltaTime;
        if (velocity.y < terminalVelocity) velocity.y = terminalVelocity;

        Vector3 finalVelocity = (moveDirection * targetSpeed) + velocity;
        controller.Move(finalVelocity * Time.deltaTime);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();

        if (health <= 0)
        {
            Debug.Log("Player Mati! Memuat GameOverLvl2...");

            // munculin cursor saat mati
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SceneManager.LoadScene("GameOverLvl2");
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

    // 🔥 beritahu MusicController kalau darah rendah
    if (MusicController.instance != null)
    {
        MusicController.instance.SetLowHealth(health <= 30);
    }
}



    // cursor muncul saat objek diancurin
    void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}