using UnityEngine;

public class Inputs : MonoBehaviour
{
    public CarControl CarControl;

    private void Awake()
    {
        CarControl = new();
    }

    private void OnEnable()
    {
        CarControl.Enable();
    }

    private void OnDisable()
    {
        CarControl.Disable();
    }
}
