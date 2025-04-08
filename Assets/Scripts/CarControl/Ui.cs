using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ui : MonoBehaviour
{
    [SerializeField] CarController _car;
    [SerializeField] TextMeshProUGUI _textMesh;
    [SerializeField] Image _speedMeter;
    [SerializeField] float _maxSpeedMeter;

    private void FixedUpdate()
    {
        _textMesh.text = $"Speed : {GetSpeed(_car.Speed)}";
    }

    float GetSpeed(float value)
    {
        value = Mathf.Round(value * 10) / 10;
        _speedMeter.fillAmount = value / _maxSpeedMeter;
        return Mathf.Round(value * 10) / 10;
    }
}
