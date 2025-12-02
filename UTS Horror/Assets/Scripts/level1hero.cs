using UnityEngine;

public class level1hero : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float sprintSpeed = 6f;
    public float rotateSpeed = 80f;
    public float mouseSensitivity = 150f;

    public Transform cam; // Drag Camera dari Inspector

    private float currentSpeed;
    private float xRotation = 0f;

    void Update()
    {
        Move();
        RotateKeyboard();
        MouseLook();
    }

    void Move()
    {
        float v = Input.GetAxis("Vertical");
        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        transform.Translate(Vector3.forward * v * currentSpeed * Time.deltaTime);
    }

    void RotateKeyboard()
    {
        float h = Input.GetAxis("Horizontal"); // A/D
        transform.Rotate(0, h * rotateSpeed * Time.deltaTime, 0);
    }

    void MouseLook()
    {
        // Mouse look aktif hanya saat klik kanan ditekan
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Rotate body left-right
            transform.Rotate(0, mouseX, 0);

            // Rotate camera up-down (pitch)
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -60f, 60f); // batasi rotasi supaya leher tidak muter 360°

            cam.localRotation = Quaternion.Euler(xRotation, 0, 0);
        }
    }
}
