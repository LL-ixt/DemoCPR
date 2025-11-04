using UnityEngine;

public class FixHead : MonoBehaviour
{
    private Vector3 initialWorldPosition;
    private Quaternion initialRotation;
    private float minAngle = 8f;
    private float thresholdAngle = 15f; // Ngưỡng góc nghiêng đầu
    public GameObject isHeadTilted; 
    void Start()
    {
        initialWorldPosition = transform.position;
        initialRotation = transform.rotation;
    }
    void LateUpdate() 
    {
        transform.position = initialWorldPosition;
        var rot = transform.rotation.eulerAngles;
        if (initialRotation.eulerAngles.z - rot.z > minAngle)
        {
            isHeadTilted.SetActive(true);
        }
        else
        {
            isHeadTilted.SetActive(false);
        }
        if (initialRotation.eulerAngles.z - rot.z > thresholdAngle) rot.z = initialRotation.eulerAngles.z - thresholdAngle;
        transform.rotation = Quaternion.Euler(initialRotation.eulerAngles.x, initialRotation.eulerAngles.y, rot.z);
    }
}
