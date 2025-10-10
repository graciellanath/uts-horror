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

    private float rotationY = 0f;
    private Transform cameraTransform;
    private CharacterController controller;

    void Start()
    {
        // Ambil kamera yang jadi anak player
        cameraTransform = GetComponentInChildren<Camera>().transform;

        // Ambil komponen CharacterController (capsule)
        controller = GetComponent<CharacterController>();

        // Kunci dan sembunyikan kursor
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
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotasi horizontal (putar badan)
        transform.Rotate(Vector3.up * mouseX);

        // Rotasi vertikal (lihat atas bawah)
        rotationY -= mouseY;
        rotationY = Mathf.Clamp(rotationY, minY, maxY);
        cameraTransform.localRotation = Quaternion.Euler(rotationY, 0, 0);
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDir = transform.right * moveX + transform.forward * moveZ;
        controller.Move(moveDir * moveSpeed * Time.deltaTime);
    }

    void UpdateCameraPosition()
    {
        // Tempatkan kamera di tengah capsule (dalam tubuh)
        Vector3 camPos = transform.position;
        camPos.y += controller.height * 0.5f - 0.1f; // sedikit di bawah atas capsule
        cameraTransform.position = camPos;
    }
}
