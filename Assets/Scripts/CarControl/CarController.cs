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
    public float Speed { get; private set; }

    [SerializeField] Inputs _inputs;
    [SerializeField] AnimationCurve _sterringCurve;
    [SerializeField] Vector3 _centerOfMass;
    [SerializeField] List<Wheel> _wheels;
    [SerializeField] float _steerMaxAngle = 35;
    [SerializeField] float _slipAngleMax = 120;
    [SerializeField] float _maxAcceleration = 1000;
    [SerializeField] float _brakeAcceleration = 3000;
    [SerializeField] float _brakeFront = 0.3f;
    [SerializeField] float _brakeRear = 0.7f;

    float _moveInput;
    float _steeringInput;
    float _slipAngle;
    float _brakeInput;
    bool _isBreaking;

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
    }

    void GetInputs()
    {
        //_moveInput = Input.GetAxis("Vertical");
        //_steeringInput = Input.GetAxis("Horizontal");
        //_isBreaking = Input.GetKey(KeyCode.Space);

        _moveInput = _inputs.CarControl.Car.Movement.ReadValue<Vector2>().y;
        _steeringInput = _inputs.CarControl.Car.Movement.ReadValue<Vector2>().x;

        float isBraking = _inputs.CarControl.Car.Brake.ReadValue<float>();

        if (isBraking != 0)
            _isBreaking = true;
        else
            _isBreaking = false;
    }

    void Move()
    {
        foreach (var wheel in _wheels)
        {
            wheel.WheelCollider.motorTorque = _moveInput * _maxAcceleration;
        }

        if (_isBreaking)
            Brake();
        else
        {
            foreach (var wheel in _wheels)
            {
                wheel.WheelCollider.brakeTorque = 0;
            }
        }
    }

    void Steer()
    {
        _slipAngle = Vector3.Angle(transform.forward, _carRb.velocity - transform.forward);

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
        float movingDirection = Vector3.Dot(transform.forward, _carRb.velocity);

        if (movingDirection != 0)
            _brakeInput = Mathf.Abs(movingDirection);

        foreach (var wheel in _wheels)
        {
            if (wheel.Axel == Axel.Rear)
                wheel.WheelCollider.brakeTorque = _brakeInput * _brakeAcceleration * _brakeRear;
            else
                wheel.WheelCollider.brakeTorque = _brakeInput * _brakeAcceleration * _brakeFront;
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

    internal void Enable()
    {
        throw new NotImplementedException();
    }

    internal void Disable()
    {
        throw new NotImplementedException();
    }
}
