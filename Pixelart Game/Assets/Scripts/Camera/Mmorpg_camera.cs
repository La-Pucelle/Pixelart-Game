using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mmorpg_camera : MonoBehaviour
{
    public Transform target;
    public float distance = 10f;
    public float sensitivity = 2f;
    public float zoomSpeed = 5f;
    public float minDistance = 2f;
    public float maxDistance = 10f;

    private float currentX = 0f;
    private float currentY = 0f;
    private bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            canMove = !canMove;
            Cursor.lockState = canMove ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !canMove;
        }

        if (canMove)
        {
            currentX += Input.GetAxis("Mouse X") * sensitivity;
            currentY -= Input.GetAxis("Mouse Y") * sensitivity;

            distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }
    }

    private void LateUpdate()
    {
        if (canMove)
        {
            currentY = Mathf.Clamp(currentY, -60f, 60f);  // Limitar el ángulo vertical

            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0f);
            transform.position = target.position - rotation * Vector3.forward * distance;
            transform.LookAt(target.position + Vector3.up);
        }
    }
}
