using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    public void PlayAgain()
    {
        SceneManager.LoadScene("Map_Hosp1");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
