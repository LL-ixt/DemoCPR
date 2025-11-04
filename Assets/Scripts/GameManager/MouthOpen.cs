using TMPro;
using UnityEngine;

public class MouthOpened : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public string text = "Há miệng: ok";
    public GameObject breathControl;
    public GameObject isHeadTilted;
    public TutorialManager tutorialManager;
    void OnEnable()
    {
        resultText.text += "\n" + text;
        if (isHeadTilted.activeInHierarchy) breathControl.GetComponent<BreathController>().enabled = true;
        tutorialManager.EnterTutorial(TutorialManager.Step.BreathPractice);
    }
    void OnDisable()
    {
        resultText.text = resultText.text.Replace("\n" + text, "");
    }
}
