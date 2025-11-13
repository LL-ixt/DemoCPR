using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToActual : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("ActualCPR");
    }
}
