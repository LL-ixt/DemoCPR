using UnityEngine;
using UnityEngine.InputSystem;

public class JawController : MonoBehaviour
{
    public Animator jawAnimator;
    public InputActionReference leftControllerButton;
    private const string IsMouthOpenParam = "isMouthOpen";
    public GameObject isMouthOpened;
    void Start()
    {
        // Đăng ký sự kiện nút
        if (leftControllerButton != null)
        {
            leftControllerButton.action.performed += OnButtonPress;
            leftControllerButton.action.canceled += OnButtonRelease;
            leftControllerButton.action.Enable();
        }
    }

    void OnDestroy()
    {
        // Hủy đăng ký khi object bị xóa để tránh memory leak
        if (leftControllerButton != null)
        {
            leftControllerButton.action.performed -= OnButtonPress;
            leftControllerButton.action.canceled -= OnButtonRelease;
        }
    }

    private void OnButtonPress(InputAction.CallbackContext context)
    {
        if (jawAnimator != null)
            jawAnimator.SetBool(IsMouthOpenParam, true);
            isMouthOpened.SetActive(true);
    }

    private void OnButtonRelease(InputAction.CallbackContext context)
    {
        if (jawAnimator != null)
            jawAnimator.SetBool(IsMouthOpenParam, false);
            isMouthOpened.SetActive(false);
    }
}
