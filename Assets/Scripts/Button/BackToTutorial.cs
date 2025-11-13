using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToTutorial : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
