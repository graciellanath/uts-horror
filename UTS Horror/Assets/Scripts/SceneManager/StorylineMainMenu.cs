using UnityEngine;
using UnityEngine.SceneManagement;

public class StorylineMainMenu : MonoBehaviour
{
    // Tombol Play
    public void StartGame()
    {
        SceneManager.LoadScene("Level1"); 
    }

    
}
