using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 3f;
    [SerializeField] private float maxVerticalAngle = 60f;
    [SerializeField] private float minVerticalAngle = -25f;
    private Vector3 mouseMovement;
    private Vector3 mouseTotalRotation; //checks total rotation done using mouse, used to clamp rotation
    private Vector3 defaultRotation = new Vector3(0f, 0f, 0f);

    private void Update()
    {
        GetMousePosition();
        ClampVerticalAngle();
    }

    void FixedUpdate()
    {
        transform.eulerAngles += mouseMovement * mouseSensitivity;
    }

    private void GetMousePosition()
    {//gets mouse x and y input.
        mouseMovement.y = Input.GetAxis(ConstantStrings.MouseX);
        mouseMovement.x = -Input.GetAxis(ConstantStrings.MouseY);
        mouseTotalRotation += mouseMovement * mouseSensitivity;
    }

    private void ClampVerticalAngle()
    {//limits vertical rotating so camera doesn't rotate through ground
        mouseTotalRotation.x = Mathf.Clamp(mouseTotalRotation.x, minVerticalAngle, maxVerticalAngle);
        transform.eulerAngles = mouseTotalRotation;
    }
}
