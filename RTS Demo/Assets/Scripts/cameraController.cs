using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField]
    float mouseSensitivity = 100;
    [SerializeField]
    float movementSpeed = 10;

    Transform parentTransform;

    float xRotation;
    private void Start()
    {
        parentTransform = transform.root;
    }
    void Update()
    {
        if (Input.GetMouseButton(2))
        {
            rotate(getMouseInputs());
        }

        move(getMovementInputs());
        
    }

    Vector3 getMovementInputs()
    {
        Vector3 moveInputs = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        return moveInputs;
    }
    Vector3 getMouseInputs()
    {
        Vector3 mouseInputs = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
        return mouseInputs;
    }
    void rotate(Vector3 inputs)
    {
        inputs *= mouseSensitivity * Time.deltaTime;

        xRotation -= inputs.y;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        parentTransform.Rotate(Vector3.up * inputs.x);
    }
    private void move(Vector3 inputs)
    {
        parentTransform.Translate(transform.TransformDirection(inputs) * movementSpeed * Time.deltaTime, Space.World);
    }
}
