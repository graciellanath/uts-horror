namespace FpsHorrorKit
{
    using UnityEngine;
    using UnityEngine.Rendering;
    // using UnityEngine.Rendering.HighDefinition; // ❌ ini untuk HDRP, hapus/comment

    public class InteractCameraSettings : MonoBehaviour
    {
        private Volume volume;
        // private DepthOfField depthOfField;  // ❌ comment
        private float startFocusDistance;

        public static InteractCameraSettings Instance;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            volume = FindAnyObjectByType<Volume>();

            // ❌ comment block ini karena URP ga punya HDRP DepthOfField
            /*
            if (volume != null && volume.profile.TryGet<DepthOfField>(out depthOfField))
            {
                startFocusDistance = depthOfField.focusDistance.value;
            }
            else
            {
                Debug.LogError("Depth of Field veya Volume bulunamadı!");
            }
            */
        }

        public void Interacting(float focusDistanceWhenInspecting = .5f)
        {
            /*
            if (depthOfField != null)
            {
                depthOfField.focusDistance.value = focusDistanceWhenInspecting;
            }
            */
        }

        public void NotInteracting()
        {
            /*
            if (depthOfField != null)
            {
                depthOfField.focusDistance.value = startFocusDistance;
            }
            */
        }

        public void ShowCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void HideCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
