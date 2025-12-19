using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class KeypadDoorSceneLoader : MonoBehaviour
{
    public string nextSceneName = "StorylineLv2";

    private bool isLoading = false;

    public void LoadNextScene()
{
    if (isLoading) return;
    isLoading = true;

    Debug.Log(">>> LOADING STORYLINE LV2 <<<");

    StartCoroutine(LoadAndDisable());
}



IEnumerator LoadAndDisable()
{
    yield return null;

    Debug.Log("🔥 ABOUT TO LOAD: " + nextSceneName);

    SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
}


}
