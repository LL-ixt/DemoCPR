using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BreathController : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    [Header("Components")]
    public AudioLoudnessDetection detector;

    [Header("Scale Settings")]
    [Tooltip("Kích thước mặc định của đối tượng BreathControl.")]
    public float defaultYScale = 1f;
    
    [Tooltip("Kích thước tối đa khi thổi (ví dụ: 1.1)")]
    public float maxYScale = 1.1f; 
    
    [Tooltip("Độ nhạy của micro (cần điều chỉnh thực tế)")]
    public float sensitivity = 150f; 
    
    [Tooltip("Ngưỡng độ lớn âm thanh tối thiểu để được coi là 'Thổi'.")]
    public float blowThreshold = 0.05f; 
    
    [Tooltip("Tốc độ đối tượng thay đổi kích thước.")]
    public float scaleSpeed = 5f;
    public int breathCount = 0;

    private float currentScale;
    private Vector3 defaultScale;
    private bool readyToCount = false;
    public TutorialManager tutorialManager;
    void OnEnable()
    {
        defaultScale = transform.localScale;
        currentScale = defaultYScale;
        detector.gameObject.SetActive(true);
    }

    void Update()
    {
        
        float currentLoudness = detector.GetLoudnessFromMicrophone();
        float loudnessFactor = currentLoudness * sensitivity;
        //Debug.Log("loudnessFactor: " + loudnessFactor);
        if (loudnessFactor > blowThreshold)
        {
            // Sử dụng SmoothStep để tạo hiệu ứng mượt mà khi phóng to
            float t = Mathf.SmoothStep(0f, 1f, loudnessFactor);
            currentScale = Mathf.Lerp(currentScale, maxYScale, t);
            if (readyToCount)
            {
                readyToCount = false;
                breathCount++;
                resultText.text += "\nThổi ngạt lần: " + breathCount;
                if (breathCount == 2)
                {
                    resultText.text += "\nĐã thổi ngạt xong.";
                    tutorialManager.EnterTutorial(TutorialManager.Step.LoopingCPR);
                    //set this game active = false;
                    gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (currentScale <= defaultYScale + 0.01f)
            {
                readyToCount = true;
            }
            // Sử dụng SmoothStep để tạo hiệu ứng mượt mà khi thu nhỏ
            currentScale = Mathf.SmoothStep(currentScale, defaultYScale, Time.deltaTime * scaleSpeed);
        }

        // Áp dụng scale mới
        transform.localScale = new Vector3(defaultScale.x, currentScale, defaultScale.z);
    }
}