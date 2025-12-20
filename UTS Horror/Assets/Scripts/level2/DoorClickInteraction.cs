using UnityEngine;

public class DoorClickInteraction : MonoBehaviour
{
    public float interactDistance = 3f;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
            {
                Debug.Log("HIT: " + hit.collider.name);

                // CARI SCRIPT DI OBJECT ATAU PARENT
                HingeDoorController door =
                    hit.collider.GetComponent<HingeDoorController>() ??
                    hit.collider.GetComponentInParent<HingeDoorController>();

                if (door != null)
                {
                    Debug.Log("DOOR FOUND - TOGGLE");
                    door.ToggleDoor();
                }
                else
                {
                    Debug.Log("NO DOOR SCRIPT ON THIS OBJECT");
                }
            }
        }
    }
}
