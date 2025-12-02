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
    public float gravity = -9.81f; // Gravitasi bumi

    [Header("Health Settings")]
    public int maxHealth = 100;
    public int health = 100;
    public TextMeshProUGUI healthText;

    private float rotationX = 0f;
    private float rotationY = 0f;
    private Transform cameraTransform;
    private CharacterController controller;
    private bool isLooking = false;

    // Variabel khusus untuk menyimpan kecepatan jatuh
    private Vector3 velocity;

    void Start()
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
        controller = GetComponent<CharacterController>();

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

        // 1. CEK APAKAH MENAPAK TANAH
        // Kalau sudah di tanah, reset kecepatan jatuh biar tidak menumpuk sampai jutaan
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Angka kecil negatif untuk memastikan tetap nempel di tanah
        }

        // 2. LOGIKA GERAK (Kanan/Kiri/Depan/Belakang)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
            currentSpeed *= sprintMultiplier;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Gerakkan karakter (Horizontal)
        controller.Move(move * currentSpeed * Time.deltaTime);

        // 3. LOGIKA GRAVITASI (Atas/Bawah)
        // Rumus fisika: v = a * t
        velocity.y += gravity * Time.deltaTime;

        // Gerakkan karakter (Vertikal/Jatuh)
        // Rumus fisika: delta_y = 1/2 * g * t^2 (diwakili velocity * Time.deltaTime disini)
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