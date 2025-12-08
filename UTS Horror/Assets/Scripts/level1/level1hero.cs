using UnityEngine;

public class level1hero : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float sprintSpeed = 6f;
    public float mouseSensitivity = 150f;

    public Transform cam; // Drag Camera dari Inspector

    private float currentSpeed;
    private float xRotation = 0f;

    void Update()
    {
        Move();
        // RotateKeyboard() dihapus karena WASD tidak boleh memutar karakter
        MouseLook();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal"); // A/D untuk Kiri/Kanan
        float v = Input.GetAxis("Vertical");   // W/S untuk Maju/Mundur

        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        // Perubahan di sini:
        // Parameter Translate sekarang menerima (x, y, z)
        // h (x) = geser samping, v (z) = maju mundur
        transform.Translate(h * currentSpeed * Time.deltaTime, 0, v * currentSpeed * Time.deltaTime);
    }

    void MouseLook()
    {
        // 1. Kunci kursor saat klik kanan ditekan awal
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // 2. Lepas kursor saat klik kanan dilepas
        if (Input.GetMouseButtonUp(1))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // 3. Putar kamera & badan HANYA saat klik kanan ditahan
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Karakter berputar Kiri-Kanan mengikuti mouse
            transform.Rotate(Vector3.up * mouseX);

            // Kamera berputar Atas-Bawah (dongak/nunduk)
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -60f, 60f);

            if (cam != null)
            {
                cam.localRotation = Quaternion.Euler(xRotation, 0, 0);
            }
        }
    }
}