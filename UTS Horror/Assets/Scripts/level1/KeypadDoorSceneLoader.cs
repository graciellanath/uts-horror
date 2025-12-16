using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class KeypadDoorSceneLoader : MonoBehaviour
{
    public string nextSceneName = "StorylineLv2";

    public void LoadNextScene()
    {
        Debug.Log(">>> LOADING STORYLINE LV2 <<<");
        StartCoroutine(LoadDelayed());
    }

    IEnumerator LoadDelayed()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextSceneName);
    }
}
