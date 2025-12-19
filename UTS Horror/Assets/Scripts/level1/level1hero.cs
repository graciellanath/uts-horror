using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]

public class level1hero : MonoBehaviour
{
    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 2f;
    public float minY = -60f;
    public float maxY = 60f;

    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;

    [Header("Audio Settings")]
    public AudioSource footstepSource;
    public AudioClip walkClip; // Input Audio 1: Jalan
    public AudioClip runClip;  // Input Audio 2: Lari

    [Header("References")]
    public Transform cam;

    [Header("Pause Settings")]
    public GameObject pauseUI;

    // Internal
    private Rigidbody rb;
    private Animator animator;
    private float rotationX = 0f;
    private float rotationY = 0f;

    // Input
    private float inputH, inputV;
    private bool isSprinting;
    private bool isPaused = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        animator = GetComponent<Animator>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (animator != null)
            animator.applyRootMotion = false;

        // --- SETUP AUDIO ---
        if (footstepSource == null)
            footstepSource = GetComponent<AudioSource>();

        // Setting default
        footstepSource.loop = true;
        footstepSource.playOnAwake = false;

        rotationX = transform.eulerAngles.y;
        if (cam != null)
            rotationY = cam.localEulerAngles.x;

        ResumeGame();
    }

    void Update()
    {
        ForceCursorVisible();

        // ESC → toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        if (isPaused) return;

        HandleMouseLook();
        HandleInput();
        HandleAnimation();
        HandleFootsteps(); // Logic audio baru
    }

    void FixedUpdate()
    {
        if (isPaused) return;
        HandleMovementPhysics();
    }

    // =========================
    // CURSOR FIX
    // =========================
    void ForceCursorVisible()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // =========================
    // MOUSE LOOK
    // =========================
    void HandleMouseLook()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            rotationX += mouseX;
            rotationY -= mouseY;
            rotationY = Mathf.Clamp(rotationY, minY, maxY);

            rb.MoveRotation(Quaternion.Euler(0, rotationX, 0));

            if (cam != null)
                cam.localRotation = Quaternion.Euler(rotationY, 0, 0);
        }
    }

    // =========================
    // INPUT
    // =========================
    void HandleInput()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");

        bool hasMovement = Mathf.Abs(inputH) > 0.1f || Mathf.Abs(inputV) > 0.1f;
        isSprinting = hasMovement && Input.GetKey(KeyCode.LeftShift);
    }

    // =========================
    // FOOTSTEP AUDIO (MODIFIED)
    // =========================
    void HandleFootsteps()
    {
        // Cek apakah ada input gerakan (WASD)
        bool isMoving = Mathf.Abs(inputH) > 0.1f || Mathf.Abs(inputV) > 0.1f;

        if (isMoving)
        {
            // Tentukan klip mana yang harus dipakai (Jalan vs Lari)
            AudioClip targetClip = isSprinting ? runClip : walkClip;

            // Logika ganti klip:
            // 1. Jika audio source sedang memainkan klip yang SALAH (misal lagi lari tapi suaranya jalan), GANTI.
            // 2. Jika audio source sedang TIDAK bunyi, NYALAKAN.
            if (footstepSource.clip != targetClip)
            {
                footstepSource.clip = targetClip;
                footstepSource.Play();
            }
            else if (!footstepSource.isPlaying)
            {
                footstepSource.Play();
            }
        }
        else
        {
            // Jika diam (tidak ada input), matikan audio
            if (footstepSource.isPlaying)
            {
                footstepSource.Stop();
            }
        }
    }

    // =========================
    // MOVEMENT
    // =========================
    void HandleMovementPhysics()
    {
        float speed = isSprinting ? runSpeed : walkSpeed;

        Vector3 moveDir = (transform.right * inputH + transform.forward * inputV).normalized;
        Vector3 velocity = moveDir * speed;

        rb.linearVelocity = new Vector3(
            velocity.x,
            rb.linearVelocity.y,
            velocity.z
        );
    }

    // =========================
    // ANIMATION
    // =========================
    void HandleAnimation()
    {
        if (animator == null) return;

        bool walking = Mathf.Abs(inputH) > 0.1f || Mathf.Abs(inputV) > 0.1f;

        animator.SetBool("isWalk", walking);
        animator.SetBool("isRun", isSprinting);
    }

    // =========================
    // PAUSE SYSTEM
    // =========================
    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        // Matikan suara langkah kaki saat pause agar tidak berdenging
        if (footstepSource.isPlaying) footstepSource.Stop();

        if (pauseUI != null)
            pauseUI.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pauseUI != null)
            pauseUI.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}