using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class StorylineLv2 : MonoBehaviour
{
    public string nextScene = "Level2";
    public Button continueButton;

    void Start()
    {
        Debug.Log(">>> STORYLINE LV2 LOADED <<<");

        // MATIKAN BUTTON SAAT SCENE LOAD
        continueButton.interactable = false;

        // AKTIFKAN SETELAH DELAY
        StartCoroutine(EnableButton());
    }

    IEnumerator EnableButton()
    {
        yield return new WaitForSeconds(0.5f);
        continueButton.interactable = true;
    }

    public void ContinueStory()
    {
        Debug.Log(">>> BUTTON CLICKED → LEVEL 2 <<<");
        SceneManager.LoadScene(nextScene);
    }
}
