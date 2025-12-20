using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
[RequireComponent(typeof(Rigidbody))]
public class HingeDoorController : MonoBehaviour
{
    private HingeJoint hinge;
    private JointMotor motor;

    private bool isOpen = false;

    [Header("Door Settings")]
    public float openVelocity = 150f;
    public float closeVelocity = -150f;
    public float motorForce = 1000f;

    void Awake()
    {
        hinge = GetComponent<HingeJoint>();

        // pastikan motor aktif lewat script
        hinge.useMotor = false;
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;

        motor = hinge.motor;
        motor.force = motorForce;

        if (isOpen)
        {
            motor.targetVelocity = openVelocity;   // buka
        }
        else
        {
            motor.targetVelocity = closeVelocity;  // tutup
        }

        hinge.motor = motor;
        hinge.useMotor = true;
    }
}
