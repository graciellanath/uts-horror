using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;

    void Awake()
    {
        // ngecek apakah ada objek musik lain?
        if (instance == null)
        {
            // jika belum ada, ini objek musik pertama
            instance = this;

            // jangan hancurkan objek saat pindah scene
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Jika sudah ada musik yang sedang main (dari scene sebelumnya),hancurin ini agar tidak bentrok.
            Destroy(gameObject);
        }
    }
}