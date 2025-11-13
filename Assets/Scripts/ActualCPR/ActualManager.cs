using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.InputSystem;
using System.Collections;
public class ActualManager : MonoBehaviour
{
    public enum Mode {Ready, Compression, Breath}
    public Mode currentMode = Mode.Ready;
    public GameObject compression;
    public GameObject breath;
    public float moveSpeed = 2.0f;
    [Header("Hiển thị text")]
    public TextMeshProUGUI modeText;
    public TextMeshProUGUI logText;
    public float patientHP = 50;
    [Header("Recenter")]
    public Transform origin;              // Gốc của toàn bộ XR Rig
    public Transform rightController;     // Tay phải của người chơi
    public Transform target;
    public Transform endingTarget;
    [Header("Input")]
    public InputActionReference recenterButton;
    public InputActionReference switchModeButton;
    public HPManager hpManager;
    //public UnityEngine.UI.Image hpBar;
    (Mode mode, int count, float score)[] logs = new (Mode, int, float)[5]
    {
        (Mode.Compression, 0, 0),
        (Mode.Breath, 0, 0),
        (Mode.Compression, 0, 0),
        (Mode.Breath, 0, 0),
        (Mode.Compression, 0, 0)
    };
    string[] logEntries = new string[5] { "", "", "", "", "" };
    private int logIndex = 0;

    void Start()
    {
        //On Ready Mode
        recenterButton.action.performed += OnRecenterPerformed(); 
        recenterButton.action.Enable();
    }

    void OnDestroy()
    {
        // Hủy đăng ký khi object bị xóa để tránh memory leak
        if (switchModeButton != null)
        {
            switchModeButton.action.performed -= OnButtonPress;
            switchModeButton.action.canceled -= OnButtonRelease;
        }
    }

    private void Recenter()
    {
        Vector3 offset = rightController.position - origin.position;
        //offset.y = 0; // chỉ dịch theo mặt phẳng ngang

        // Di chuyển origin sao cho tay phải nằm đúng vị trí target
        origin.position = target.position - offset;

        // Xoay origin sao cho hướng của tay phải trùng với hướng của target
        Vector3 targetForward = target.forward;
        targetForward.y = 0;
        Vector3 handForward = rightController.forward;
        handForward.y = 0;

        float angle = Vector3.SignedAngle(handForward, targetForward, Vector3.up);
        origin.RotateAround(rightController.position, Vector3.up, angle);
        
        recenterButton.action.performed -= OnRecenterPerformed();
        recenterButton.action.Disable();
        SwitchMode();
        // Đăng ký sự kiện nút
        if (switchModeButton != null)
        {
            switchModeButton.action.performed += OnButtonPress;
            switchModeButton.action.canceled += OnButtonRelease;
            switchModeButton.action.Enable();
        }
    
    }
    private Action<InputAction.CallbackContext> OnRecenterPerformed()
    {
        return context => Recenter();
    }


    public void SwitchMode()
    {
        currentMode = (currentMode != Mode.Compression) ? Mode.Compression : Mode.Breath;
        if (currentMode == Mode.Compression)
        {
            compression.SetActive(true);
            breath.SetActive(false);
            modeText.text = "Bạn đang: Ép ngực";
        }
        else
        {
            compression.SetActive(false);
            breath.SetActive(true);
            modeText.text = "Bạn đang: Thổi ngạt";
            //UpdateLog(Mode.Breath, 100);
        }
    }

    public void RegisterAction(string actionType, int count, float score)
    {
        // Cập nhật HP dựa vào điểm
        patientHP += score * 0.5f; 
        patientHP = Mathf.Clamp(patientHP, 0, 100);

        //UpdateLog($"{actionType}: {count} lần - Điểm {score}%");
        //UpdateHPBar();

        if (patientHP >= 100)
            SceneManager.LoadScene("ResultScene_Success");
        else
            SceneManager.LoadScene("ResultScene_Fail");
    }

    public void UpdateLog(Mode mode, float score)
    {
        //Debug.Log($"Updating log for mode {mode} with score {score}");
        if (logs[0].mode == mode)
        {
            logs[0] = (mode, logs[0].count + 1, score);
        }
        else
        {
            for (int i = logs.Length - 1; i > 0; i--)
            {
                logs[i] = logs[i - 1];
            }
            logs[0] = (mode, 1, score);
        }

        //Đếm số lần của logs[0] để biết có đúng không?
        if (logs[0].count > 2 && mode == Mode.Breath)
        {
            hpManager.AddHP(-5);
        }
        else if (logs[0].count > 30 && mode == Mode.Compression)
        {
            hpManager.AddHP(-5);
        }
        else hpManager.AddHP(score * 0.1f);
            

        string fullLog = "5 thao tác gần đây:\n";
        for (int i = 0; i < logs.Length; i++)
        {
            if (logs[i].count > 0)
            {
                if (logs[i].mode == Mode.Compression)
                    logEntries[i] = $"Ép ngực: {logs[i].count} lần - Điểm {logs[i].score}%";
                else
                    logEntries[i] = $"Thổi ngạt: {logs[i].count} lần - Điểm {logs[i].score}%";
                fullLog += logEntries[i] + "\n";
            }
        }
        logText.text = fullLog;
    }
    private void OnButtonPress(InputAction.CallbackContext context)
    {
        //Debug.Log("Button Pressed - Switching Mode");
        SwitchMode();
    }

    private void OnButtonRelease(InputAction.CallbackContext context)
    {
        //Debug.Log("Button Released - Switching Mode");
        SwitchMode();
    }

    //Translate Tracking Space to ending target
    public void GoAway()
    {
        Vector3 newPosition = endingTarget.position;
        newPosition.y = origin.position.y;
        endingTarget.position = newPosition;
        compression.SetActive(false);
        breath.SetActive(false);
        StartCoroutine(MoveToEndingTarget());
    }

    private IEnumerator MoveToEndingTarget()
    {
        while (Vector3.Distance(origin.position, endingTarget.position) > 0.01f)
        {
            origin.position = Vector3.MoveTowards(
                origin.position,
                endingTarget.position,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }
        origin.position = endingTarget.position;
    }
}
