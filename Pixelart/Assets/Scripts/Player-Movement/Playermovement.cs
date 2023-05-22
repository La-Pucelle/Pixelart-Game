using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    private Animator animator;
    public float speed = 50f;
    private Rigidbody rb;
    public Transform modelTransform;
    private Transform cameraTransform;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal") * Time.deltaTime;
        float verticalInput = Input.GetAxis("Vertical") * Time.deltaTime;

        if (Input.GetKey("q"))
        {
            animator.SetBool("smileFlag", true);
        }
        else
        {
            animator.SetBool("smileFlag", false);
        }

        if (Input.GetKey("space"))
        {
            animator.SetBool("jumpFlag", true);
            animator.SetBool("walkFlag", false);
            animator.SetBool("idleFlag", false);
        }
        else
        {
            animator.SetBool("jumpFlag", false);
            animator.SetBool("idleFlag", true);
        }

        if ((Input.GetKey("up")) || (Input.GetKey("right")) || (Input.GetKey("down")) || (Input.GetKey("left")) || Input.GetKey("w") || Input.GetKey("d") || Input.GetKey("s") || Input.GetKey("a"))
        {
            animator.SetBool("jumpFlag", false);
            animator.SetBool("walkFlag", true);
            animator.SetBool("idleFlag", false);
        }
        else
        {
            animator.SetBool("walkFlag", false);
            animator.SetBool("idleFlag", true);
        }

        // Obtener la dirección de movimiento relativa a la cámara
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0f; // Esto asegura que el movimiento sea horizontal
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calcular el vector de movimiento
        Vector3 movement = (cameraForward * verticalInput + cameraRight * horizontalInput).normalized;

        // Aplicar el movimiento al jugador
        rb.MovePosition(transform.position + movement * speed * Time.deltaTime);
        if (movement.magnitude > 0f)
        {
            // Calcular la rotación hacia la dirección del movimiento
            Quaternion targetRotation = Quaternion.LookRotation(movement);

            // Aplicar suavemente la rotación al modelo del jugador
            modelTransform.rotation = Quaternion.Lerp(modelTransform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }
}