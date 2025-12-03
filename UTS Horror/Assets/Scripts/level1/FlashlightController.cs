using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Light flashlight;
    private bool isOn = false;

    void Start()
    {
        if (flashlight == null)
            Debug.LogWarning("Flashlight BELUM di-assign ke script!");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // klik kiri
        {
            isOn = !isOn; // toggle
            flashlight.enabled = isOn;

            if (isOn)
                Debug.Log("Flashlight: ON");
            else
                Debug.Log("Flashlight: OFF");
        }
    }
}
