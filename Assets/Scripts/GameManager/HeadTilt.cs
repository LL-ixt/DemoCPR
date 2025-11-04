using TMPro;
using UnityEngine;

public class HeadTilt : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public string text = "Nghiêng đầu: ok";
    void OnEnable()
    {
        resultText.text += "\n" + text;
    }
    void OnDisable()
    {
        resultText.text = resultText.text.Replace("\n" + text, "");
    }
}
