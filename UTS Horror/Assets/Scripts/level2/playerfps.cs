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

    // Internal
    private float rotationX;
    private float rotationY;
    private Transform cameraTransform;
    private CharacterController controller;
    private Animator animator;
    private bool isLooking;
    private Vector3 velocity;
    private bool isDead = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = GetComponentInChildren<Camera>().transform;
        animator = GetComponentInChildren<Animator>();

        // LOCK cursor saat main
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        health = maxHealth;
        UpdateHealthUI();

        rotationX = transform.eulerAngles.y;
        rotationY = cameraTransform.localEulerAngles.x;
    }

    void Update()
    {
        if (Time.timeScale == 0f || isDead) return;

        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        if (Input.GetMouseButtonDown(1)) isLooking = true;
        if (Input.GetMouseButtonUp(1)) isLooking = false;

        if (!isLooking) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX += mouseX;
        rotationY -= mouseY;
        rotationY = Mathf.Clamp(rotationY, minY, maxY);

        transform.rotation = Quaternion.Euler(0, rotationX, 0);
        cameraTransform.localRotation = Quaternion.Euler(rotationY, 0, 0);
    }

    void HandleMovement()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        bool hasInput = Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f;
        bool sprint = hasInput && Input.GetKey(KeyCode.LeftShift);

        float speed = sprint ? runSpeed : walkSpeed;

        if (animator != null)
        {
            animator.SetBool("isWalk", hasInput);
            animator.SetBool("isRun", sprint);
        }

        Vector3 move = (transform.right * h + transform.forward * v).normalized;
        velocity.y += gravity * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, terminalVelocity);

        controller.Move((move * speed + velocity) * Time.deltaTime);
    }

    // ================= HEALTH =================

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();

        if (health <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
    }

    void Die()
    {
        isDead = true;

        Debug.Log("PLAYER MATI → PINDAH KE GAME OVER");

        // UNLOCK cursor sebelum pindah scene
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // pastiin game jalan normal
        Time.timeScale = 1f;

        SceneManager.LoadScene("GameOverLv2");
    }

    void UpdateHealthUI()
    {
        if (healthText == null) return;

        healthText.text = $"HEALTH: {health}%";

        if (health > 70) healthText.color = Color.green;
        else if (health > 30) healthText.color = Color.yellow;
        else healthText.color = Color.red;
    }

    void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
