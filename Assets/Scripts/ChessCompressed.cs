using System;
using Oculus.Interaction.Input;
using UnityEngine;

public class ChessCompressed : MonoBehaviour
{
    public Transform rightController; // tay phải
    public float chestTopY;           // đỉnh ngực (ban đầu)
    public float chestBottomY;
    public float chestCountY;        // ngưỡng để tính số lần
    public float minScaleY = 0.8f;
    private bool isCompressing = false;
    private bool canBeCounted = true; // đếm số lần ép
    void Start()
    {
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("RightHand"))
        {
            isCompressing = true;
            HandleCompression();
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RightHand"))
        {
            isCompressing = false;
        }
    }

    private void HandleCompression()
    {
        //Debug.Log("Handling Compression!!!");
        float controllerY = rightController.position.y;

        // chỉ xử lý nếu tay đang trong khoảng ép
        if (controllerY < chestTopY && controllerY > chestBottomY)
        {
            float compression = chestTopY - controllerY;
            float normalized = Mathf.Clamp01(compression / (chestTopY - chestBottomY));

            float newScaleY = Mathf.Lerp(1f, minScaleY, normalized);
            transform.localScale = new Vector3(1f, newScaleY, 1f);
        }
        else if (controllerY <= chestBottomY)
        {
            // tay quá thấp → giữ nguyên scale min
            transform.localScale = new Vector3(1f, minScaleY, 1f);
        }

        if (canBeCounted && controllerY < chestTopY)
        {
            canBeCounted = false;
        }
        else if (controllerY >= chestTopY)
        {
            canBeCounted = true; // reset để có thể đếm lần ép tiếp theo
        }
    }

    void Update()
    {
        if (!isCompressing)
        {
            // khi không ép, trở về scale ban đầu
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
        }
        //Debug.Log("Controller Y: " + rightController.position.y);
    }
}
