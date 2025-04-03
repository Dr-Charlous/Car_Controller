using TMPro;
using UnityEngine;

public class Ui : MonoBehaviour
{
    [SerializeField] CarController _car;
    [SerializeField] TextMeshProUGUI _textMesh;

    private void FixedUpdate()
    {
        _textMesh.text = $"Speed : {GetSpeed(_car.Speed)}";
    }

    float GetSpeed(float value)
    {
        return Mathf.Round(value*10)/10;
    }
}
