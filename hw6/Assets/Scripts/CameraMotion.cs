using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{
    [SerializeField] private string mouseX, mouseY;
    [SerializeField] private float mouseSensitivity;
    private float xClamp;
    [SerializeField] private Transform playerPos;


    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Start is called before the first frame update
    void Awake()
    {
        LockCursor();
        xClamp = 0f;
    }

    // Update is called once per frame
    void Update()
    {

        CameraRotation();
    }


    private void CameraRotation()
    {
        float mousX = Input.GetAxis(mouseX) * mouseSensitivity * Time.deltaTime;
        float mousY = Input.GetAxis(mouseY) * mouseSensitivity * Time.deltaTime;

        xClamp += mousY;
        if (xClamp > 90)
        {
            xClamp = 90f;
            mousY = 0;
            clampXAxis(270.0f);

        }
        else if (xClamp < -90)
        {
            xClamp = -90f;
            mousY = 0;
            clampXAxis(90.0f);

        }
        transform.Rotate(Vector3.left * mousY);
        playerPos.Rotate(Vector3.up * mousX);
    }

    private void clampXAxis(float v)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = v;
        transform.eulerAngles = eulerRotation;
    }
}
