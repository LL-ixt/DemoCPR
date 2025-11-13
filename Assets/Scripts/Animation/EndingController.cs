using Oculus.Interaction;
using UnityEngine;

public class EndingController : MonoBehaviour
{
    public Animator animator; // Gán Animator của nhân vật nạn nhân
    public SceneTransitionManager sceneTransitionManager;
    public GameObject head;
    //private bool hasPlayed = false;

    // Hàm gọi khi bấm nút Win
    public void PlayWakeUp()
    {
        //if (hasPlayed) return;

        //hasPlayed = true;
        head.GetComponent<GrabInteractable>().enabled = false;
        head.GetComponent<Grabbable>().enabled = false;
        head.GetComponent<FixHead>().enabled = false;
        head.GetComponent<BoxCollider>().enabled = false;
        animator.SetTrigger("WakeUp");  // Trigger animation tỉnh dậy
    }
}
