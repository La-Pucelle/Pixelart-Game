using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    private Animator animator;
    public float speed = 2f;
    private Rigidbody rb;
    private Transform cameraTransform;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKey("q"))
        {
            animator.SetBool("smileFlag", true);
        }
        else
        {
            animator.SetBool("smileFlag", false);
        }

        if (Input.GetKeyDown("space"))
        {
            animator.SetBool("jumpFlag", true);
            animator.SetBool("walkFlag", false);
            animator.SetBool("idleFlag", false);
        }

        if ((Input.GetKey("up")) || (Input.GetKey("right")) || (Input.GetKey("down")) || (Input.GetKey("left")) || Input.GetKey("w") || Input.GetKey("d") || Input.GetKey("s") || Input.GetKey("a"))
        {
            animator.SetBool("jumpFlag", false);
            animator.SetBool("walkFlag", true);
            animator.SetBool("idleFlag", false);
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
        rb.MovePosition(transform.position + movement * speed);
    }
}