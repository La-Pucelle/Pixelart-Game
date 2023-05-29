using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    public float speed = 5f;
    float additionalFactor = 5f;
    private Rigidbody rb;
    public Transform modelTransform;
    private Transform cameraTransform;
    private Rigidbody rigid;

    public float jumpForce = 5f;
    public LayerMask groundLayer;
    public float raycastDistance = 1.1f;
    private bool isGrounded = false;
    private bool isCrouching = false;



    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal") * Time.deltaTime;
        float verticalInput = Input.GetAxis("Vertical") * Time.deltaTime;


        // Obtener la dirección de movimiento relativa a la cámara
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0f; // Esto asegura que el movimiento sea horizontal
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        
        // Calcular el vector de movimiento
        Vector3 movement = (cameraForward * verticalInput + cameraRight * horizontalInput).normalized;

        // Salto
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }


        // Aplicar el movimiento al jugador
        rb.MovePosition(transform.position + movement * speed * additionalFactor * Time.deltaTime);
        if (movement.magnitude > 0f)
        {
            // Calcular la rotación hacia la dirección del movimiento
            Quaternion targetRotation = Quaternion.LookRotation(movement);

            // Aplicar suavemente la rotación al modelo del jugador
            modelTransform.rotation = Quaternion.Lerp(modelTransform.rotation, targetRotation, 10f * Time.deltaTime);
        }
        // Correr
        if (Input.GetKey(KeyCode.LeftShift))
        {
            additionalFactor = 10f;
        }
        else
        {
            additionalFactor = 5f;
        }

        

        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
        }
        //ajustar altura del personaje :o

        if (isCrouching)
        {
            additionalFactor = 2.5f;
            modelTransform.localScale = new Vector3(modelTransform.localScale.x, 0.5f, modelTransform.localScale.z);
        }
        else
        {
            additionalFactor = 5f;
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
