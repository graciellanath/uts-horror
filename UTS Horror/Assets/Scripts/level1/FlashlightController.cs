using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Light flashlight;
    private bool isOn = false;

    void Start()
    {
        if (flashlight == null)
            Debug.LogWarning("Flashlight BELUM di-assign ke script!");

        // Pastikan senter OFF saat game mulai
        flashlight.enabled = false;
        isOn = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // tekan F
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
    