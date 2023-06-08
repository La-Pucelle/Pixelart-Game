using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rb; // Rigidbody del modelo
    public Transform modelTransform; // Rotacion del modelo
    private Transform cameraTransform; // Direccion de la camara

    public float raycastDistance = 1.1f; // Distancia del rayo
    public LayerMask groundLayer;
    private bool isGrounded = false; 
    private bool isCrouching = false;



    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


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
       

        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
        }
        //ajustar altura del personaje :o

        if (isCrouching)
        {
            modelTransform.localScale = new Vector3(modelTransform.localScale.x, 0.5f, modelTransform.localScale.z);
        }
        else
        {
            modelTransform.localScale = new Vector3(modelTransform.localScale.x, 1f, modelTransform.localScale.z);
        }
    }

    void FixedUpdate()
    {
        // Lanzar un rayo hacia abajo para detectar colisiones con el suelo
        if (Physics.Raycast(transform.position, Vector3.down, raycastDistance, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
