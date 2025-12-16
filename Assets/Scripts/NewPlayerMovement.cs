using JetBrains.Annotations;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;


public class NewPlayerMovement : MonoBehaviour
{
    enum movement
    {
        N=3, S=-3, E=1,W=-1,
        NE=4, NW=2, SE= -2, SW=-4,

    }

    public bool cutsceneEnded = false;
    // Riferimenti
    private CharacterController characterController;
    private Animator animator;

    public float jumpheight;
    // Movement Speed
    public float speed;

    // mouse Sensibility
    public float mouseSensitivity;
    
    // Gravity (for the jump)
    public float gravity;
    private Vector3 velocity;
    public float dashspeed = 100f;



    private float rotationX = 0f;
    //used to avoid double attack or attacking while jumping
    private bool isAttacking;

    private bool isDashing = false;
    private float dashingDirectionUD;
    private float dashingDirectionLR;
    private bool DashinCD = false;

    public TextMeshProUGUI CDText;
    public GameObject rollImage;


    movement moves;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        // locking view in the middle
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        isAttacking = false;    
        animator.SetBool("strongAA", false);
        
        
    }

    

    void Update()
    {
        if (FightManager.Instance.cutSceneEnded && !FightManager.Instance.cardChosing)
        {
            Debug.Log("Movimento in corso");
            speed = 10f;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = 20f;
            }

            // === MOVIMENTO CON FRECCETTE ===
            float moveX = Input.GetAxis("Horizontal"); // Freccia sinistra/destra
            float moveZ = Input.GetAxis("Vertical");   // Freccia su/gi�

            // === GESTIONE DELLA GRAVIT� ===
            if (characterController.isGrounded && velocity.y < 0)
            {
                // Debug.Log("Decreasing");
                velocity.y = -5f; // Tiene il personaggio a terra
            }
        ;
            //Debug.Log("VELOCITY.Y = "+velocity.y);
            // Calcola la direzione del movimento basata sulla rotazione del giocatore

            if (Input.GetKeyDown(KeyCode.LeftControl) && !isDashing && !DashinCD)
            {



                float RL = Input.GetAxisRaw("Horizontal");
                float UD = Input.GetAxisRaw("Vertical") * 3;
                int sum = (int)(RL + UD);
                Debug.Log(sum);
                Debug.Log((movement)sum);
                dashingDirectionUD = UD / 3;
                dashingDirectionLR = RL;

                if (sum != 0)
                {
                    Debug.Log("STARTING RECHARGE");
                    EndAttack();
                    DashRecharge();
                    isDashing = true;
                }



                CheckMovement(sum);

            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("DASH") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
            {
                Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);


                moveZ = dashingDirectionUD;
                moveX = dashingDirectionLR;
                speed = dashspeed;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("DASH") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f)
            {
                isDashing = false;
            }



            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            characterController.Move(move * speed * Time.deltaTime);




            velocity.y += gravity * Time.deltaTime;

            characterController.Move(velocity * Time.deltaTime);
            //Debug.Log(characterController.isGrounded);
            //Debug.Log("Velocit� verticale: " + velocity.y);
            // Salto
            if (characterController.isGrounded && Input.GetKeyDown(KeyCode.Space) && !isDashing)
            {
                animator.SetBool("isJump", true);
                animator.SetBool("isAttacking", true);
                Debug.Log("Pressed Spacebar");
                velocity.y = Mathf.Sqrt(jumpheight * -2f * gravity);
                isAttacking = true;
            }
            else
            {
                animator.SetBool("isJump", false);

            }

            if (Input.GetMouseButtonDown(0))
            {
                if (!isAttacking)
                {
                    Attack("lightAttack");
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                if (!isAttacking)
                {
                    Attack("strongAttack");
                }
            }

            if (isAttacking && animator.GetCurrentAnimatorStateInfo(0).IsTag("AA") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {

                EndAttack();
            }

            if (isAttacking && animator.GetCurrentAnimatorStateInfo(0).IsTag("JUMP") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f)
            {

                EndAttack();
            }











            // === ROTAZIONE CON IL MOUSE ===
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            // Ruota il personaggio orizzontalmente (sinistra/destra)
            transform.Rotate(Vector3.up * mouseX);



            // Ruota la visuale verticalmente (su/gi�) con limite
            //rotationX -= mouseY;
            //rotationX = Mathf.Clamp(rotationX, -23f, 16f);



            //animation
            bool walking = moveX != 0 || moveZ != 0;
            animator.SetFloat("verticalDir", moveZ);
            animator.SetFloat("horizontalDir", moveX);
            animator.SetBool("isRun", Input.GetKey(KeyCode.LeftShift));
            animator.SetBool("isWalk", walking);
        }
        
    }
    void Attack(string attackType)
    {
        Debug.Log("ATTACK TYPE " + attackType);
        isAttacking = true;  // Blocca altri attacchi
        animator.SetTrigger(attackType);
        animator.SetBool("isAttacking", true);
        isDashing = false;
    }

    // Metodo chiamato da un Animation Event alla fine dell'attacco
    public void EndAttack()
    {
        Debug.Log("End ATTACK");
        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }


    void CheckMovement(int sum)
    {
        switch ((movement)sum)
        {
            case movement.N:
                animator.SetTrigger("dodgeU");
                Debug.Log("Movimento: Nord");
                break;
            case movement.S:
                animator.SetTrigger("dodgeD");
                Debug.Log("Movimento: Sud");
                break;
            case movement.E:
                animator.SetTrigger("dodgeR");
                Debug.Log("Movimento: Est");
                break;
            case movement.W:
                animator.SetTrigger("dodgeL");
                Debug.Log("Movimento: Ovest");
                break;
            case movement.NE:
                animator.SetTrigger("dodgeU");
                Debug.Log("Movimento: Nord-Est");
                break;
            case movement.NW:
                animator.SetTrigger("dodgeU");
                Debug.Log("Movimento: Nord-Ovest");
                break;
            case movement.SE:
                animator.SetTrigger("dodgeD");
                Debug.Log("Movimento: Sud-Est");
                break;
            case movement.SW:
                animator.SetTrigger("dodgeD");
                Debug.Log("Movimento: Sud-Ovest");
                break;
            default:
                Debug.Log("Movimento non valido");
                break;
        }
    }

    private void DashRecharge()
    {
        DashinCD = true;
        StartCoroutine(StartCountdown(3));
    }


    IEnumerator StartCountdown(int cooldown)
    {

        rollImage.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
        while (cooldown > 0)
        {
            Debug.Log("DashCooldown "+ cooldown);
            CDText.text= cooldown.ToString();
          
            yield return new WaitForSeconds(1f);
            cooldown--;
        }

        CDText.text = "";       
        //yield return new WaitForSeconds(1f);
        DashinCD = false;
        rollImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

}