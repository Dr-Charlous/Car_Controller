using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    [SerializeField] Transform _rotTransform;
    [SerializeField] Vector3 _speed;

    private void Start()
    {
        _rotTransform.rotation = Quaternion.Euler(_speed.normalized * Random.Range(0, 180));
    }

    private void FixedUpdate()
    {
        _rotTransform.Rotate(_speed);
    }
}
