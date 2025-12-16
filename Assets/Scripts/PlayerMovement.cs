using JetBrains.Annotations;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Riferimenti
    private CharacterController characterController;
    private Animator animator;

    public float jumpheight = 50f;
    // Movement Speed
    public float speed = 50f;

    // mouse Sensibility
    public float mouseSensitivity = 5f;

    // Gravity (for the jump)
    public float gravity = -2f;
    private Vector3 velocity;

    
    private float rotationX = 0f;
    public Transform playerCamera;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        // locking view in the middle
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        speed = 50f;
        if (Input.GetKey(KeyCode.LeftShift)){
            speed = 100f;
        }
       
        // === MOVIMENTO CON FRECCETTE ===
        float moveX = Input.GetAxis("Horizontal"); // Freccia sinistra/destra
        float moveZ = Input.GetAxis("Vertical");   // Freccia su/giù
        
        // === GESTIONE DELLA GRAVITÀ ===
        if (characterController.isGrounded && velocity.y < 0)
        {
           // Debug.Log("Decreasing");
            velocity.y = -5f; // Tiene il personaggio a terra
        }
        ;
        //Debug.Log("VELOCITY.Y = "+velocity.y);
        // Calcola la direzione del movimento basata sulla rotazione del giocatore
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(move * speed * Time.deltaTime);



        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
        Debug.Log(characterController.isGrounded);
        Debug.Log("Velocità verticale: " + velocity.y);
        // Salto
        if (characterController.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            
            Debug.Log("Pressed Spacebar");
            velocity.y = Mathf.Sqrt(jumpheight * -2f * gravity);
        }
       

        // === ROTAZIONE CON IL MOUSE ===
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Ruota il personaggio orizzontalmente (sinistra/destra)
        transform.Rotate(Vector3.up * mouseX);

        // Ruota la visuale verticalmente (su/giù) con limite
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -23f, 16f);
        playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        


        //animation
        bool walking = moveX != 0 || moveZ != 0;
        animator.SetBool("isRunning", Input.GetKey(KeyCode.LeftShift));
        animator.SetBool("isWalking", walking);
        animator.SetBool("isJumping", characterController.isGrounded);
    }
}