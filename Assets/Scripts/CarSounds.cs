using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSounds : MonoBehaviour
{
    public float MinSpeed;
    public float MaxSpeed;
    float _currentSpeed;

    Rigidbody _carRb;
    AudioSource _carAudio;

    public float MinPitch;
    public float MaxPitch;
    float _pitchFromCar;

    void Start()
    {
        _carAudio = GetComponent<AudioSource>();
        _carRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        EngineSound();
    }

    void EngineSound()
    {
        _currentSpeed = _carRb.velocity.magnitude;
        _pitchFromCar = _carRb.velocity.magnitude / 50;

        if (_currentSpeed < MinSpeed )
        {
            _carAudio.pitch = MinPitch;
        }

        if (_currentSpeed > MinSpeed && _currentSpeed < MaxSpeed)
        {
            _carAudio.pitch = MinPitch + _pitchFromCar;
        }

        if (_currentSpeed > MaxSpeed)
        {
            _carAudio.pitch = MaxPitch;
        }
    }
}
