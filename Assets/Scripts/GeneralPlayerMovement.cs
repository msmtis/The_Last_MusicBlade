using JetBrains.Annotations;
using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.UIElements;
using UnityEngine.UI;


public class GeneralPlayerMovement : MonoBehaviour
{

    public PlayerInput _playerInput;
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
    public int dashCooldown = 3;




    //NEW
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isRunning;
    private bool isWalking;
    private float RunSpeed;
    private float currentSpeed;
    //private bool SwordBox;
    public Collider SwordCollider;


    movement moves;
    void Start()
    {
        isRunning = false;
        isWalking = false;
        currentSpeed = speed;
        RunSpeed = 2 * speed;
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        // locking view in the middle
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        isAttacking = false;
        animator.SetBool("strongAA", false);
        
        SwordCollider.enabled = false;
        
        
        
    }



    void Update()
    {

        if (FightManager.Instance.cutSceneEnded && !FightManager.Instance.cardChosing)
        {
             Debug.Log("Movimento in atto");
            UIManager.Instance.StartFight();
            //MOVEMENT SECTION
            float moveX = moveInput.x;
            float moveZ = moveInput.y;
            
            Vector3 move = transform.right * moveX + transform.forward * moveZ;

            //DASHING SECTION
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("DASH")){
                Debug.Log("NORMALIZED TIME" + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("DASH") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
            {
                moveZ = dashingDirectionUD;
                moveX = dashingDirectionLR;
                currentSpeed = dashspeed;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("DASH") &&
                     animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f)
            {
                isDashing = false;
                currentSpeed = speed;
            }

            Debug.Log(move);
            Debug.Log(moveX);

            //APPLYING MOVEMENT
            characterController.Move(move * currentSpeed * Time.deltaTime);
            
            

            // rotazione con il mouse (lookInput)
            float mouseX = lookInput.x * mouseSensitivity;
            transform.Rotate(Vector3.up * mouseX);


            //JUMPING SECTION
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);


            //CHECK ATTACK CD
            if (isAttacking && animator.GetCurrentAnimatorStateInfo(0).IsTag("AA") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                
                EndAttack();
            }

            if (isAttacking && animator.GetCurrentAnimatorStateInfo(0).IsTag("JUMP") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f)
            {

                EndAttack();
            }


            animator.SetFloat("verticalDir", moveZ);
            animator.SetFloat("horizontalDir", moveX);
            animator.SetBool("isRun", isRunning);
            animator.SetBool("isWalk", isWalking);

        }
    }
    
    void Attack(string attackType)
    {
        SwordCollider.enabled = true;
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
        StartCoroutine(StartCountdown(dashCooldown));
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


    public void OnMove(InputAction.CallbackContext context) 
    {
        currentSpeed = speed;
        if (context.performed)
        {
            
            isWalking = true;
            moveInput = context.ReadValue<Vector2>();

        }else if (context.canceled)
        {

            isWalking = false;
            moveInput = Vector2.zero;
        }
        
    }

    public void onLook(InputAction.CallbackContext context)
    {
        lookInput= context.ReadValue<Vector2>();   
    }

    public void onJump(InputAction.CallbackContext context)
    {

        Debug.Log("GROUNDED "+characterController.isGrounded);
        if (context.performed && characterController.isGrounded && !isDashing)
        {
            Debug.Log("IS JUMPING");
            velocity.y = Mathf.Sqrt(jumpheight * -2f * gravity);
            animator.SetBool("isJump", true);
            animator.SetBool("isAttacking", true);
            isAttacking = true;
        }
        if (context.canceled)
        {
            isAttacking = false;
            animator.SetBool("isJump", false);
            animator.SetBool("isAttacking", false);
        }
    }
       

    public void onDash(InputAction.CallbackContext context)
    {
        //check if pressed, check if is already dashing, check if dash is available
        if (context.started && !isDashing && !DashinCD)
        {
            int sum = (int)(moveInput.x + moveInput.y * 3);
            dashingDirectionUD = moveInput.y;
            dashingDirectionLR = moveInput.x;

            if (sum != 0)
            {
                EndAttack();
                DashRecharge();
                isDashing = true;
            }

            CheckMovement(sum);
        }
        
    }

    public void OnLightAttack(InputAction.CallbackContext context)
    {
        if (context.performed && !isAttacking)
        {
            Attack("lightAttack");
        }
    }

    public void OnStrongAttack(InputAction.CallbackContext context)
    {
        if (context.performed && !isAttacking)
        {
            Attack("strongAttack");
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("IS RUNNING");
            currentSpeed = RunSpeed;
            isRunning = true;
            

        }
        else if (context.canceled)
        {
            Debug.Log("IS NOT RUNNING");
            isRunning = false;
            currentSpeed = speed;
        }
    }

}