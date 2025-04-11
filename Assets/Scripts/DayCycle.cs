using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    [SerializeField] Transform _rotTransform;
    [SerializeField] Vector3 _speed;
    [SerializeField] GameObject _day, _night;
    [SerializeField] bool _isDay, _isNight;

    private void Start()
    {
        if (!_isDay && !_isNight)
            _rotTransform.localRotation = Quaternion.Euler(Vector3.right * Random.Range(0, 180));
    }

    private void FixedUpdate()
    {
        _rotTransform.Rotate(_speed);

        if (_rotTransform.localEulerAngles.x > 0 && _rotTransform.localEulerAngles.x < 180)
        {
            _isDay = true;
            _isNight = false;
        }
        else if (_rotTransform.localEulerAngles.x > 180 && _rotTransform.localEulerAngles.x < 360)
        {
            _isDay = false;
            _isNight = true;
        }

        if (_day.activeSelf != _isDay)
            _day.SetActive(_isDay);
        if (_night.activeSelf != _isNight)
            _night.SetActive(_isNight);

        if (_rotTransform.localEulerAngles.x >= 360)
            _rotTransform.localRotation = Quaternion.Euler(Vector3.zero);
    }
}
