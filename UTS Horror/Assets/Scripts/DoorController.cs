using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    public Animator doorAnimator;
    public float interactDistance = 3f;

    [Header("References")]
    public Transform player;

    private bool isOpen = false;
    private bool isAnimating = false;

    void Update()
    {
        if (player == null || isAnimating) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance < interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            if (PlayerHasKey.hasKey && !isOpen)
            {
                StartCoroutine(OpenTheDoor());
            }
            else if (!PlayerHasKey.hasKey)
            {
                Debug.Log("🚪 Pintu terkunci! Kamu butuh kunci.");
            }
        }
    }

    IEnumerator OpenTheDoor()
    {
        isAnimating = true;
        isOpen = true;
        doorAnimator.SetBool("adaKunci", true);
        Debug.Log("🔑 Pintu berhasil dibuka dengan kunci!");
        yield return new WaitForSeconds(1f);
        isAnimating = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOpen && other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Win");
        }
    }
}