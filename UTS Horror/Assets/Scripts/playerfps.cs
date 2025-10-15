using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // ⬅️ Ganti dari UnityEngine.UI ke TextMeshPro

[RequireComponent(typeof(CharacterController))]
public class playerfps : MonoBehaviour
{
    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 2f;
    public float minY = -60f;
    public float maxY = 60f;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float sprintMultiplier = 2f;

    [Header("Health Settings")]
    public int maxHealth = 100;
    public int health = 100;
    public TextMeshProUGUI healthText; // ⬅️ TMP Text untuk UI Health

    private float rotationY = 0f;
    private Transform cameraTransform;
    private CharacterController controller;
    private bool isLooking = false;

    void Start()
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        UpdateHealthUI(); // tampilkan health saat mulai
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        UpdateCameraPosition();
    }

    void HandleMouseLook()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isLooking = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isLooking = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (isLooking)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            transform.Rotate(Vector3.up * mouseX);
            rotationY -= mouseY;
            rotationY = Mathf.Clamp(rotationY, minY, maxY);
            cameraTransform.localRotation = Quaternion.Euler(rotationY, 0, 0);
        }
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= sprintMultiplier;
        }

        Vector3 moveDir = transform.right * moveX + transform.forward * moveZ;
        controller.Move(moveDir * currentSpeed * Time.deltaTime);
    }

    void UpdateCameraPosition()
    {
        Vector3 camPos = transform.position;
        camPos.y += controller.height * 0.5f - 0.1f;
        cameraTransform.position = camPos;
    }

    // 🩸 Fungsi menerima damage
    public void TakeDamage(int amount)
    {
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);

        UpdateHealthUI();

        if (health <= 0)
        {
            Debug.Log("Player mati!");
            SceneManager.LoadScene("GameOver");
        }
    }

    // 💊 Fungsi menambah darah saat ambil first aid
    public void Heal(int amount)
    {
        if (health >= maxHealth)
        {
            Debug.Log("Darah sudah penuh! Tidak bisa menggunakan First Aid.");
            return;
        }

        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);

        Debug.Log($"Menggunakan First Aid, darah sekarang: {health}%");
        UpdateHealthUI();
    }

    // 🧠 Update tampilan UI (dengan warna dinamis)
    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {health}%";

            // Ubah warna berdasarkan persentase darah
            if (health > 70)
                healthText.color = Color.green;
            else if (health > 30)
                healthText.color = Color.yellow;
            else
                healthText.color = Color.red;
        }
    }
}
