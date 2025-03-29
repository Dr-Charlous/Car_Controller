using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    [SerializeField] Vector3 _speed;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(_speed.normalized * Random.Range(0, 180));
    }

    private void FixedUpdate()
    {
        transform.Rotate(_speed);
    }
}
