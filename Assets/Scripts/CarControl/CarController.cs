using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

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

    [SerializeField] float _steerMaxAngle = 35;
    [SerializeField] float _slipAngleMax = 120;
    [SerializeField] float _maxAcceleration = 200;
    [SerializeField] float _breakAcceleration = 500000;
    [SerializeField] Vector3 _centerOfMass;
    [SerializeField] List<Wheel> _wheels;
    [SerializeField] AnimationCurve _sterringCurve;

    float _moveInput;
    float _steeringInput;
    float _slipAngle;
    public float Speed { get; private set; }
    float _brakeInput;

    Rigidbody _carRb;

    private void Start()
    {
        _carRb = GetComponent<Rigidbody>();
        _carRb.centerOfMass = _centerOfMass;
    }

    private void Update()
    {
        GetInputs();
        AnimationWheels();

        Speed = _carRb.velocity.magnitude;
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

        _slipAngle = Vector3.Angle(transform.forward, _carRb.velocity - transform.forward);

        float movingDirection = Vector3.Dot(transform.forward, _carRb.velocity);
        if (movingDirection < -0.5f && _moveInput > 0)
        {
            _brakeInput = Mathf.Abs(_moveInput);
        }
        else if (movingDirection > 0.5f && _moveInput < 0)
        {
            _brakeInput = Mathf.Abs(_moveInput);
        }
        else
        {
            _brakeInput = 0;
        }
    }

    void Move()
    {
        foreach (var wheel in _wheels)
        {
            wheel.WheelCollider.motorTorque = _moveInput * _maxAcceleration;
        }
    }

    void Steer()
    {

        float steeringAngle = _steeringInput * _sterringCurve.Evaluate(Speed);
        if (_slipAngle < _slipAngleMax)
        {
            steeringAngle += Vector3.SignedAngle(transform.forward, _carRb.velocity + transform.forward, Vector3.up);
        }
        steeringAngle = Mathf.Clamp(steeringAngle, -_steerMaxAngle, _steerMaxAngle);

        foreach (var wheel in _wheels)
        {
            if (wheel.Axel == Axel.Front)
                wheel.WheelCollider.steerAngle = steeringAngle;
        }
    }


    void Brake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            foreach (var wheel in _wheels)
            {
                if (wheel.Axel == Axel.Rear)
                    wheel.WheelCollider.brakeTorque = _brakeInput * _breakAcceleration * 0.7f;
                else
                    wheel.WheelCollider.brakeTorque = _brakeInput * _breakAcceleration * 0.3f;
            }
        }
    }

    void AnimationWheels()
    {
        foreach (var wheel in _wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.WheelCollider.GetWorldPose(out pos, out rot);
            wheel.WheelModel.transform.position = pos;
            wheel.WheelModel.transform.rotation = rot;
        }
    }
}
