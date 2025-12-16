using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform player;
    public LayerMask WhatIsPlayer;
    private Animator animator;
    [SerializeField] private Collider swordCollider;


    //Attacking 
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    // Start is called before the first frame update
    
    
    
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (FightManager.Instance.cutSceneEnded && !FightManager.Instance.cardChosing)
        {
            Debug.Log("Nemico in movimento");
            //Check if the player is in attack or sight range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, WhatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, WhatIsPlayer);

            if (playerInSightRange && !playerInAttackRange) { chasePlayer(); }
            if (playerInSightRange && playerInAttackRange) { attackPlayer(); }
        }
        

    }

    private void chasePlayer() {
        if (!alreadyAttacked)
        {
            animator.SetBool("isChasing", true);
            agent.SetDestination(player.position);
            Debug.Log("is Chasing");
        }
       
    }
    private void attackPlayer() {
        
        //make sure the enemy stopping to move
        transform.LookAt(player);
        agent.SetDestination(transform.position);
       

        if (!alreadyAttacked)
        {
            int randomNumber = Random.Range(1, 3);
            switch (randomNumber)
            {
                case (1):
                    animator.SetTrigger("Attack1");
                    swordCollider.enabled = true;
                    break;
                case (2):
                    animator.SetTrigger("Attack3");
                    swordCollider.enabled = true;
                    break;
                /*case (3):
                    
                    animator.SetTrigger("Attack3");
                    break;
                */


            }
                     
            
            
            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }


    private void ResetAttack() { 
        alreadyAttacked= false;
    }
}
