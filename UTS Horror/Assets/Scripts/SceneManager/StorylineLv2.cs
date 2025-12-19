using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems; // ← TAMBAHKAN INI

public class StorylineLv2 : MonoBehaviour
{
    public string nextScene = "Level2";
    public Button continueButton;

    private bool canContinue = false;

    void Start()
    {
        Debug.Log(">>> STORYLINE LV2 LOADED <<<");

        // 🔴 PENTING: HAPUS SELECTED BUTTON AGAR TIDAK AUTO-SUBMIT
        EventSystem.current.SetSelectedGameObject(null);

        continueButton.interactable = false;
        StartCoroutine(EnableButton());
    }

    IEnumerator EnableButton()
    {
        yield return new WaitForSeconds(0.5f);
        canContinue = true;
        continueButton.interactable = true;
    }

    public void ContinueStory()
    {
        if (!canContinue) return;

        Debug.Log(">>> BUTTON CLICKED → LEVEL 2 <<<");
        SceneManager.LoadScene(nextScene);
    }

    void Awake()
{
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    if (player != null)
        player.SetActive(false);

         Debug.Log("🚨 STORYLINE LV2 AWAKE");
}

}
