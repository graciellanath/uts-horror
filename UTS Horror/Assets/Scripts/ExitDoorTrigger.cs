using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoorTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerHasKey.hasKey)
            {
                Debug.Log("🎉 Player berhasil keluar!");
                SceneManager.LoadScene("Win"); // Ganti dengan nama scene kamu
            }
            else
            {
                Debug.Log("🚪 Kamu butuh kunci untuk keluar!");
            }
        }
    }
}
