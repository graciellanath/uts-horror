using UnityEngine;

public class VaultDoorController : MonoBehaviour
{
    public Transform door;
    public float slideDistance = 2f;
    public float speed = 2f;

    private bool shouldOpen = false;
    private Vector3 initialPos;
    private Vector3 targetPos;

    void Start()
    {
        initialPos = door.localPosition;
        targetPos = initialPos + new Vector3(slideDistance, 0, 0);
    }

    // DIPANGGIL OLEH KEYPAD (OnAccessGranted)
    public void OpenDoor()
    {
        shouldOpen = true;
    }

    void Update()
    {
        if (!shouldOpen) return;

        door.localPosition = Vector3.Lerp(
            door.localPosition,
            targetPos,
            Time.deltaTime * speed
        );
    }
}
