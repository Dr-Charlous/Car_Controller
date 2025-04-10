using System;
using System.Collections.Generic;
using UnityEngine;

public class CarControl2 : MonoBehaviour
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

    //[SerializeField] float _steerMaxAngle = 35;
    //[SerializeField] float _slipAngleMax = 120;
    //[SerializeField] float _maxAcceleration = 200;
    //[SerializeField] float _brakeAcceleration = 500000;
    //[SerializeField] Vector3 _centerOfMass;
    //[SerializeField] AnimationCurve _sterringCurve;

    //float _slipAngle;
    //public float Speed { get; private set; }
    //float _brakeInput;

    //Rigidbody _carRb;





    [SerializeField] List<Wheel> _wheels;
    [SerializeField] float _motorForce;
    [SerializeField] float _breakForce;
    [SerializeField] float _maxSteeringAngle;

    float _moveInput;
    float _steeringInput;
    float _currentBreakForce;
    float _currentSteerAngle;
    bool _isBreaking;

    private void Start()
    {
        //_carRb = GetComponent<Rigidbody>();
        //_carRb.centerOfMass = _centerOfMass;
    }

    private void Update()
    {
        GetInputs();
        AnimationWheels();

        //Speed = _carRb.velocity.magnitude;
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
        _isBreaking = Input.GetKey(KeyCode.Space);

        //_slipAngle = Vector3.Angle(transform.forward, _carRb.velocity - transform.forward);
    }

    void Move()
    {
        foreach (var wheel in _wheels)
        {
            if (wheel.Axel == Axel.Front)
                wheel.WheelCollider.motorTorque = _moveInput * _motorForce;
        }

        _currentBreakForce = _isBreaking ? _breakForce : 0f;

        if (_isBreaking)
        {
            Brake();
        }
    }

    void Steer()
    {
        //float steeringAngle = _steeringInput * _sterringCurve.Evaluate(Speed);
        //if (_slipAngle < _slipAngleMax)
        //{
        //    steeringAngle += Vector3.SignedAngle(transform.forward, _carRb.velocity + transform.forward, Vector3.up);
        //}
        //steeringAngle = Mathf.Clamp(steeringAngle, -_steerMaxAngle, _steerMaxAngle);

        _currentSteerAngle = _maxSteeringAngle * _steeringInput;

        foreach (var wheel in _wheels)
        {
            if (wheel.Axel == Axel.Front)
                wheel.WheelCollider.steerAngle = _currentSteerAngle;
        }
    }


    void Brake()
    {
        foreach (var wheel in _wheels)
        {
            wheel.WheelCollider.brakeTorque = _currentBreakForce;
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