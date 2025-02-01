using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float MoveSmoothness;
    public float RotSmoothness;

    public Vector3 MoveOffset;
    public Vector3 RotOffset;

    public Transform CarTarget;

    private void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        Vector3 targetPos = new Vector3();
        targetPos = CarTarget.TransformPoint(MoveOffset);

        transform.position = Vector3.Lerp(transform.position, targetPos, MoveSmoothness * Time.deltaTime);
    }

    void HandleRotation()
    {
        var direction = CarTarget.position - transform.position;
        var rotation = new Quaternion();

        rotation = Quaternion.LookRotation(direction + RotOffset, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, RotSmoothness * Time.deltaTime);
    }
}
