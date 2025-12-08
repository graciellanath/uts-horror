using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class KeypadDoorSceneLoader : MonoBehaviour
{
    public string nextSceneName = "Level2";

    public void LoadNextScene()
    {
        StartCoroutine(LoadDelayed());
    }

    IEnumerator LoadDelayed()
    {
        yield return new WaitForSeconds(1f); // delay animasi pintu
        SceneManager.LoadScene(nextSceneName);
    }
}
