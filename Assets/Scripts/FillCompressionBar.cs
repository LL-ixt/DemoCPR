using UnityEngine;
using UnityEngine.UI;

public class CompressionBarFill : MonoBehaviour
{
    [Header("References")]
    public Transform rightController;  // Tay phải VR
    public float chestTopY = 1.02f;    // Tay ở vị trí cao nhất
    public float chestBottomY = 0.9f;  // Tay ép sâu nhất

    private Image fillImage;           // Hình ảnh (Image) của chính object này

    private bool isGoingDown = false;
    private bool reachedBottom = false;

    private float lastNormal;
    private float normal = 0.1667f;

    // Biến tính toán
    public int count = 0;
    public int compressScore = 0;
    public float averageCompressScore = 0f;

    private float lastCompressionTime = 0f;
    public float curBPM = 0f;
    public float averageBPM = 0f;
    public float bpmScore = 0f;
    public float finalScore = 0f;

    void Start()
    {
        count = 0;
        float controllerY = rightController.localPosition.y;
        float normalized = Mathf.InverseLerp(chestTopY, chestBottomY, controllerY);
        normalized = Mathf.Clamp01(normalized);
        lastNormal = normalized;
        fillImage = GetComponent<Image>();
        lastCompressionTime = Time.time;
    }

    void Update()
    {
        float controllerY = rightController.localPosition.y;
        Debug.Log("Controller Y: " + controllerY);
        //Debug.Log("isGoingDown: " + isGoingDown + "reachedBottom: " + reachedBottom);
        // Chuẩn hóa vị trí tay thành [0,1]
        float normalized = Mathf.InverseLerp(chestTopY, chestBottomY, controllerY);
        normalized = Mathf.Clamp01(normalized);
        fillImage.fillAmount = normalized;
        //Debug.Log("Normalized: " + normalized + ", normal: " + normal);
        if (normalized <= normal)
        {
        }
        else if (normalized > lastNormal + 0.001f)
        {
            isGoingDown = true;
            reachedBottom = false;
        }
        else if (normalized < lastNormal - 0.001f) // tay đang đi lên
        {
            if (isGoingDown && !reachedBottom)
            {
                // === Khi đạt đáy ===
                reachedBottom = true;
                isGoingDown = false;
                count++;

                // --- TÍNH BỎNG NHỊP (BPM) ---
                float curTime = Time.time;
                float deltaTime = curTime - lastCompressionTime;
                lastCompressionTime = curTime;

                curBPM = 60f / deltaTime; // tính theo giây → phút
                averageBPM = (averageBPM * (count - 1) + curBPM) / count;

                // --- TÍNH ĐIỂM ---
                compressScore = CountScore(lastNormal); // độ sâu
                averageCompressScore = (averageCompressScore * (count - 1) + compressScore) / count;

                bpmScore = BPMScore(averageBPM);
                finalScore = (bpmScore + averageCompressScore) / 2f;
            }
        }
        lastNormal = normalized;
    }

    int CountScore(float normalized)
    {
        // Ép quá sâu hoặc quá nông → điểm thấp
        if (normalized < 0.583f) return 50; // hơi nông
        if (normalized < 0.667f) return 100; // lý tưởng
        if (normalized < 0.75f) return 40; // hơi sâu
        return -100; // quá sâu
    }

    float BPMScore(float bpm)
    {
        // Theo AHA: lý tưởng 100–120 BPM
        if (bpm >= 100f && bpm <= 120f)
            return 100f;
        else if (bpm < 60f || bpm > 160f)
            return 0f;
        else if (bpm < 100f)
            return Mathf.Lerp(0f, 100f, (bpm - 60f) / 40f);
        else // bpm > 120
            return Mathf.Lerp(100f, 0f, (bpm - 120f) / 40f);
    }
}