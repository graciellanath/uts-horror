using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class playerfps : MonoBehaviour
{
    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 2f;
    public float minY = -60f;
    public float maxY = 60f;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float sprintMultiplier = 2f; // kecepatan tambahan saat sprint

    private float rotationY = 0f;
    private Transform cameraTransform;
    private CharacterController controller;

    private bool isLooking = false; // apakah sedang mode look (klik mouse ditekan)

    void Start()
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        UpdateCameraPosition();
    }

    void HandleMouseLook()
    {
        // Aktifkan mode look saat mouse kanan ditekan (atau kiri, bisa diganti)
        if (Input.GetMouseButtonDown(1)) // 1 = klik kanan, 0 = klik kiri
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

        // Hanya rotasi kamera kalau sedang mode look
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
}
