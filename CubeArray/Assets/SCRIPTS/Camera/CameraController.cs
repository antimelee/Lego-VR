﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private static float movementSpeed = 1.0f;

    void Update()
    {
        movementSpeed = Mathf.Max(movementSpeed += Input.GetAxis("Mouse ScrollWheel"), 0.0f);
        transform.position += (transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical") + transform.up * Input.GetAxis("Depth")) * movementSpeed;
        transform.eulerAngles += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), Input.GetAxis("Rotation"));
    }
}
