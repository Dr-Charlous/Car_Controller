using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject WheelModel;
        public WheelCollider WheelCollider;
        public Axel Axel;
    }

    public float MaxAcceleration = 30;
    public float BreakAcceleration = 50;

    public float TurnSensitivity = 1;
    public float MaxSteeringAngle = 30;

    public Vector3 CenterOfMass;


    public List<Wheel> Wheels;

    float _moveInput;
    float _steeringInput;

    Rigidbody _carRb;

    private void Start()
    {
        _carRb = GetComponent<Rigidbody>();
        _carRb.centerOfMass = CenterOfMass;
    }

    private void Update()
    {
        GetInputs();
        AnimationWheels();
    }

    private void FixedUpdate()
    {
        Move();
        Steer();
        Brake();
    }

    void GetInputs()
    {
        _moveInput = Input.GetAxis("Vertical");
        _steeringInput = Input.GetAxis("Horizontal");
    }

    void Move()
    {
        foreach (var wheel in Wheels)
        {
            wheel.WheelCollider.motorTorque = _moveInput * 600 * MaxAcceleration * Time.deltaTime;
        }
    }

    void Steer()
    {
        foreach(var wheel in Wheels)
        {
            if (wheel.Axel == Axel.Front)
            {
                var steerAngle = _steeringInput * TurnSensitivity * MaxSteeringAngle;
                wheel.WheelCollider.steerAngle = Mathf.Lerp(wheel.WheelCollider.steerAngle, steerAngle, 0.6f);
            }
        }
    }

    void Brake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            foreach (var wheel in Wheels)
            {
                wheel.WheelCollider.brakeTorque = 300 * BreakAcceleration * Time.deltaTime;
            }
        }
        else
        {
            foreach (var wheel in Wheels)
            {
                wheel.WheelCollider.brakeTorque = 0;
            }
        }
    }

    void AnimationWheels()
    {
        foreach (var wheel in Wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.WheelCollider.GetWorldPose(out pos, out rot);
            wheel.WheelModel.transform.position = pos;
            wheel.WheelModel.transform.rotation = rot;
        }
    }
}
