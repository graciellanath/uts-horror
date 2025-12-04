using UnityEngine;

public class SafeDoorController : MonoBehaviour
{
    public Animator doorAnim;

    public void OpenDoor()
    {
        if (doorAnim != null)
        {
            doorAnim.SetTrigger("Open");
        }
        else
        {
            Debug.LogWarning("Animator belum diassign!");
        }
    }
}
