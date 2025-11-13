using UnityEngine;

public class BreathMode : MonoBehaviour
{
    public ActualBreathController actualBreathController;
    void OnEnable()
    {
        Debug.Log("BreathMode OnEnable");
        actualBreathController.enabled = true;
    }

    void OnDisable()
    {
        Debug.Log("BreathMode OnDisable");
        actualBreathController.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
