using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    private Animator animator;
    public float speed = 100f;
    float additionalFactor = 5f;
    private Rigidbody rb;
    public Transform modelTransform;
    private Transform cameraTransform;

    public float jumpForce = 50f;
    private bool isGrounded = true;
    private bool isCrouching = false;



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


        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false; // El jugador está en el aire después de saltar
            }
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
    
    //pa saber si esta en el suelowo
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
