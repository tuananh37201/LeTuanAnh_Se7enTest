using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothTime = 0.3f;

    private Vector3 cameraVelocity = Vector3.zero;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        MoveCamera();
    }

    public void MoveCamera()
    {
        if (target != null)
        {
            Vector3 targetPostion = target.position + offset;
            // dịch chuyển camera đến vị trí mục tiêu
            transform.position = Vector3.SmoothDamp(transform.position, targetPostion, ref cameraVelocity, smoothTime);
        }
    }

}
