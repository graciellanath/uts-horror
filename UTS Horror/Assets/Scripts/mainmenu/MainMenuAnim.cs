using UnityEngine;
using TMPro;
using System.Collections;

public class HospitalMenuAnim : MonoBehaviour
{
    [Header("References")]
    public GameObject titleObject;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI playText;
    public GameObject howToPlayObject;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public float skipSilence = 0.0f;
    public float startDelay = 3.0f;

    [Header("Timing Settings")]
    public float minInterval = 5.0f;
    public float maxInterval = 8.0f;
    public float glitchDuration = 2.0f;

    // Variable internal
    private Color originalTitleColor;
    private Vector3 originalTitleScale;
    private Vector3 originalTitlePos;

    void Start()
    {
        // matiin suara awal
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }

        // save original states
        if (titleObject != null)
        {
            originalTitleScale = titleObject.transform.localScale;
            originalTitlePos = titleObject.transform.localPosition;
        }
        if (titleText != null) originalTitleColor = titleText.color;
        if (playText != null) playText.color = Color.red;

        StartCoroutine(TitleGlitchCutRoutine());
        StartCoroutine(PlayButtonElectricFailure()); 
        MakeHowToPlayScary();
    }

    IEnumerator PlayButtonElectricFailure()
    {
        while (true)
        {
            if (playText != null)
            {
                float chance = Random.Range(0f, 100f);

                if (chance < 60)
                {
                    // 60% Waktu terang
                    playText.alpha = 1.0f;
                    yield return new WaitForSeconds(Random.Range(0.2f, 0.8f));
                }
                else if (chance < 85)
                {
                    // 25% REDUP 
                    playText.alpha = Random.Range(0.2f, 0.5f);
                    yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
                }
                else if (chance < 95)
                {
                    //Blackout sebentar
                    playText.alpha = 0f;
                    yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
                }
                else
                {
                    //Nyala-Mati cepet banget
                    for (int i = 0; i < 5; i++) // Kedip 5 kali cepet
                    {
                        playText.alpha = 0f; // Mati
                        yield return new WaitForSeconds(0.03f);
                        playText.alpha = 1f; // Nyala
                        yield return new WaitForSeconds(0.03f);
                    }
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator TitleGlitchCutRoutine()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            if (audioSource != null)
            {
                audioSource.Stop();
                audioSource.time = skipSilence;
                audioSource.Play();
            }

            float timer = 0;
            while (timer < glitchDuration)
            {
                if (titleText != null)
                {
                    Color[] glitchColors = { Color.red, Color.white, Color.gray, Color.black, Color.cyan };
                    titleText.color = glitchColors[Random.Range(0, glitchColors.Length)];
                }

                if (titleObject != null)
                {
                    float shakeX = Random.Range(-20f, 20f);
                    float shakeY = Random.Range(-20f, 20f);
                    titleObject.transform.localPosition = originalTitlePos + new Vector3(shakeX, shakeY, 0);

                    float scaleX = Random.Range(0.8f, 1.5f);
                    float scaleY = Random.Range(0.8f, 1.5f);
                    titleObject.transform.localScale = new Vector3(originalTitleScale.x * scaleX, originalTitleScale.y * scaleY, 1);
                }

                yield return new WaitForSeconds(0.05f);
                timer += 0.05f;
            }

            if (audioSource != null) audioSource.Stop();

            if (titleText != null) titleText.color = originalTitleColor;
            if (titleObject != null)
            {
                titleObject.transform.localScale = originalTitleScale;
                titleObject.transform.localPosition = originalTitlePos;
            }

            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
        }
    }

    void MakeHowToPlayScary()
    {
        iTween.ScaleTo(howToPlayObject, iTween.Hash(
            "x", 1.2f, "y", 1.2f, "time", 2.0f,
            "looptype", iTween.LoopType.pingPong, "easetype", iTween.EaseType.easeInOutSine
        ));

        iTween.RotateTo(howToPlayObject, iTween.Hash(
            "z", 5f, "time", 3.0f,
            "looptype", iTween.LoopType.pingPong, "easetype", iTween.EaseType.easeInOutSine
        ));
    }
}