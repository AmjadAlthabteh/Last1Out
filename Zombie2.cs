using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie2 : MonoBehaviour
{
    [Header("Zombie Health and Damage")]
    private float zombieHealth = 100f;
    private float presentHealth;
    public float giveDamage = 5f;

    [Header("Zombie Things")]
    public NavMeshAgent zombieAgent;
    public Transform LookPoint;
    public Camera AttackingRaycastArea;
    public Transform playerBody;
    public LayerMask PlayerLayer;

    [Header("Zombie Standing var")]
    public float zombieSpeed = 2f;

    [Header("Zombie Attacking Var")]
    public float timeBtwAttack;
    bool previouslyAttack;

    [Header("Zombie Animations")]
    public Animator anim;

    [Header("Zombie mood/states")]
    public float visionRadius;
    public float attackingRadius;
    public bool playerInvisionRadius;
    public bool playerInattackingRadius;

    private void Awake()
    {
        presentHealth = zombieHealth;
        zombieAgent = GetComponent<NavMeshAgent>();
        zombieAgent.speed = zombieSpeed; // Ensure zombie moves at set speed

    }

    private void Update()
    {
        // Check if the player is in vision or attack range
        playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
        playerInattackingRadius = Physics.CheckSphere(transform.position, attackingRadius, PlayerLayer);

        if (!playerInvisionRadius && !playerInattackingRadius)
        {
            Idle(); // Patrol when no player is in sight
        }
        if (playerInvisionRadius && !playerInattackingRadius)
        {
            Pursueplayer(); // Chase the player when seen
        }
        if (playerInattackingRadius && playerInattackingRadius)
        {
            AttackPlayer();
        }
    }

    private void Idle()
    {
        zombieAgent.SetDestination(transform.position);
    }

    private void Pursueplayer()
    {
        if (zombieAgent.SetDestination(playerBody.position))
        {
            //animation
            //anim.SetBool("Walking", false);
            //anim.SetBool("Running", true);
            //anim.SetBool("Attacking", false);
            //anim.SetBool("Died", false);
        }
        else
        {
            //anim.SetBool("Walking", false);
            //anim.SetBool("Running", false);
            //anim.SetBool("Attacking", false);
            //anim.SetBool("Died", true);
        }
    }
    private void AttackPlayer()
    {

        zombieAgent.SetDestination(transform.position);
        transform.LookAt(LookPoint);
        if (!previouslyAttack)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(AttackingRaycastArea.transform.position, AttackingRaycastArea.transform.forward, out hitInfo, attackingRadius))
            {
                Debug.Log("Attacking" + hitInfo.transform.name);

                PlayerScript playerBody = hitInfo.transform.GetComponent<PlayerScript>();

                if (playerBody != null)
                {
                    playerBody.playerHitDamage(giveDamage);
                }
                //anim.SetBool("Attacking", true);
                //anim.SetBool("Walking", false);
                //anim.SetBool("Running", false);
                //anim.SetBool("Died", false);
            }

            previouslyAttack = true;
            Invoke(nameof(ActiveAttacking), timeBtwAttack);
        }
    }

    private void ActiveAttacking()
    {
        previouslyAttack = false;
    }

    public void zombieHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;

        if (presentHealth <= 0)
        {

            //anim.SetBool("Walking", false);
            //anim.SetBool("Running", false);
            //anim.SetBool("Attacking", false);
            //anim.SetBool("Died", true);

            zombieDie();
        }
    }

    private void zombieDie()
    {
        zombieAgent.SetDestination(transform.position);
        zombieSpeed = 0f;
        attackingRadius = 0f;
        visionRadius = 0f;
        playerInattackingRadius = false;
        playerInvisionRadius = false;
        Object.Destroy(gameObject, 5.0f);
    }

}
