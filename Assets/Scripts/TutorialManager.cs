using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;
using System;
using UnityEngine.SceneManagement;
public class TutorialManager : MonoBehaviour
{
    public enum Step
    {
        Intro,
        ExplainCPR,
        CompressionSetup,
        CompressionPractice,
        CompressionComplete,
        BreathSetup,
        BreathPractice,
        LoopingCPR,
        GoToActualScene
    }
    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI progressText;
    public Button nextButton;
    public GameObject compressionBar;

    [Header("Recenter")]
    public Transform rightController;
    public Transform origin;
    public Transform target;
    public InputActionProperty recenterButton;
    public SceneTransitionManager sceneTransitionManager;
    
    void Start()
    {
        EnterTutorial(Step.Intro);
        compressionBar.SetActive(false);
    }


    void Update()
    {

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
        EnterTutorial(Step.CompressionPractice);
    }
    public void EnterTutorial(Step step)
    {
        Step currentStep = step;
        nextButton.onClick.AddListener(() => EnterTutorial(step + 1));
        switch (step)
        {
            case Step.Intro:
                instructionText.text = "Đây là hướng dẫn CPR. Nhấn nút Next để bắt đầu.";
                nextButton.gameObject.SetActive(true);
                break;
            case Step.ExplainCPR:
                instructionText.text = "CPR bao gồm ép tim và thổi ngạt. Nhấn Next để tiếp tục.";
                break;
            case Step.CompressionSetup:
                instructionText.text = "Đặt Right Controller lên một vật mô phỏng ngực. Nhấn A để tự căn chỉnh vị trí";
                nextButton.gameObject.SetActive(false);
                //Await for Recenter button
                recenterButton.action.performed += OnRecenterPerformed(); 
                recenterButton.action.Enable();
                break;
            case Step.CompressionPractice:
                instructionText.text =  "Đan hai lòng bàn tay và ấn xuống ngực. \n Bạn sẽ được chấm điểm dựa trên tốc độ và độ sâu";
                progressText.gameObject.SetActive(true);
                compressionBar.SetActive(true);
                progressText.text = "Số lần ép:";
                //Disable
                recenterButton.action.performed -= OnRecenterPerformed();
                recenterButton.action.Disable();
                break;
            case Step.CompressionComplete:
                instructionText.text = "Bạn đang làm tốt rồi đấy. ép đủ 30 lần để chuyển sang thổi ngạt.";
                break;
            case Step.BreathSetup:
                instructionText.text = "Sau đây là thổi ngạt. Tay phải grab để ngửa đầu nạn nhân. Tay trái giữ X để mở miệng.";
                break;
            case Step.BreathPractice:
                instructionText.text = "Cúi xuống miệng nạn nhân và thổi vào miệng 2 lần.";
                break;
            case Step.LoopingCPR:
                instructionText.text = "Lặp lại chu kỳ 30 ép và 2 thổi cho đến khi nạn nhân tỉnh lại.";
                nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start CPR";
                nextButton.gameObject.SetActive(true);
                break;
            case Step.GoToActualScene:
                sceneTransitionManager.GoToSceneAsync("ActualCPR");
                break;
        }
    }

    private Action<InputAction.CallbackContext> OnRecenterPerformed()
    {
        return context => Recenter();
    }
}