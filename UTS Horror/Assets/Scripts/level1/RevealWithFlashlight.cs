using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class RevealWithFlashlight : MonoBehaviour
{
    public Light flashlight;           // drag spotlight di Inspector
    public float revealDistance = 12f; // jarak maksimal agar terlihat
    [Range(0f,1f)] public float hiddenAlpha = 0.05f;
    [Range(0f,1f)] public float visibleAlpha = 1f;
    public float fadeSpeed = 6f;

    Renderer rend;
    Color matColor;

    void Start()
    {
        rend = GetComponent<Renderer>();
        // buat material instance supaya perubahan alpha hanya mempengaruhi object ini
        rend.material = new Material(rend.material);
        matColor = rend.material.color;
        matColor.a = hiddenAlpha;
        rend.material.color = matColor;

        if (flashlight == null)
            Debug.LogWarning("RevealWithFlashlight: assign flashlight (Spot Light) in Inspector.");
    }

    void Update()
    {
        if (flashlight == null) return;

        // kalau lampu mati -> sembunyikan
        if (!flashlight.enabled)
        {
            FadeTo(hiddenAlpha);
            return;
        }

        Vector3 toObj = transform.position - flashlight.transform.position;
        float dist = toObj.magnitude;

        // cek range
        if (dist > flashlight.range) { FadeTo(hiddenAlpha); return; }

        // cek angle (cone)
        float halfAngle = flashlight.spotAngle * 0.5f;
        float angle = Vector3.Angle(flashlight.transform.forward, toObj);
        if (angle > halfAngle) { FadeTo(hiddenAlpha); return; }

        // occlusion check: raycast dari lampu ke object
        Ray ray = new Ray(flashlight.transform.position, toObj.normalized);
        if (Physics.Raycast(ray, out RaycastHit hit, dist))
        {
            // jika objek pertama yang kena adalah objek ini atau child -> reveal
            if (hit.collider != null && (hit.collider.transform == transform || hit.collider.transform.IsChildOf(transform)))
                FadeTo(visibleAlpha);
            else
                FadeTo(hiddenAlpha);
        }
        else
        {
            // tidak kena collider apa-apa (rare) -> reveal
            FadeTo(visibleAlpha);
        }
    }

    void FadeTo(float targetAlpha)
    {
        matColor = rend.material.color;
        matColor.a = Mathf.Lerp(matColor.a, targetAlpha, Time.deltaTime * fadeSpeed);
        rend.material.color = matColor;
    }
}
