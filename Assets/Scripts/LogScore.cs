using TMPro;
using UnityEngine;

public class LogScore : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    private CompressionBarFill compressionBar;

    void Start()
    {
        // Tìm đối tượng CompressionBarFill trong scene (chỉ tìm 1 lần)
        compressionBar = FindAnyObjectByType<CompressionBarFill>();
    }

    void Update()
    {
        if (compressionBar != null && resultText != null)
        {
            resultText.text = "Count: " + compressionBar.count 
            + "\nScore: " + compressionBar.compressScore + "\nAverage Score: " + compressionBar.averageCompressScore.ToString("F2")
            + "\nBPM: " + compressionBar.curBPM.ToString("F2") + "\nAverage BPM: " + compressionBar.averageBPM.ToString("F2")
            + "\nBPM Score: " + compressionBar.bpmScore.ToString("F2") + "\nFinal Score: " + compressionBar.finalScore.ToString("F2");
        }
    }
}
