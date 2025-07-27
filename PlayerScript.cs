using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Mouse Look")]
    public float mouseSensitivity = 200f;

    [Header("Player Health Things")]
    private float playerHealth = 120f;
    public float presentHealth;

    [Header("Player Movement")]
    public float playerSpeed = 1.9f;
    public float playerSprint = 3.5f; // Slightly increased sprint speed

    [Header("Player Script Cameras")]
    public Transform playerCamera;

    [Header("Player Animator and Gravity")]
    public CharacterController cC;
    public float gravity = -9.81f;
    public Animator animator;

    [Header("Player Jump and Velocity")]
    public float jumpRange = 1f;
    Vector3 velocity;
    public bool onSurface;
    public float turnCalmTime = 0.1f;
    float turnCalmVelocity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        presentHealth = playerHealth;
    }

    private void Update()
    {
        // ✅ Debug Ground Detection
        bool wasOnSurface = onSurface;
        onSurface = cC.isGrounded || velocity.y < -0.5f; // Prevent false negatives


        if (wasOnSurface != onSurface) // Only log when it changes
            Debug.Log("onSurface Changed: " + onSurface);

        if (onSurface)
        {
            velocity.y = -2f; // Keep character grounded
        }
        else
        {
            velocity.y += gravity * Time.deltaTime; // Apply gravity
        }

        cC.Move(new Vector3(0, velocity.y, 0) * Time.deltaTime);

        Sprint();
        playerMove();
        Jump();

        // ✅ Debug Sprint Animation
        bool isRunning = animator.GetBool("Running");
        Debug.Log("Running Animation Active: " + isRunning);
    }




    void playerMove()
    {
        float horizontal_axis = Input.GetAxisRaw("Horizontal");
        float vertical_axis = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

        if (direction.magnitude >= 0.1f)
        {
            Vector3 moveDirection = playerCamera.forward * vertical_axis + playerCamera.right * horizontal_axis;
            moveDirection.y = 0f;
            moveDirection.Normalize();

            // Rotate player towards movement direction
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), 10f * Time.deltaTime);

            // ✅ Fix: Ensure Walk animation plays if not sprinting
            if (!animator.GetBool("Running"))
            {
                animator.SetBool("Walk", true);
                animator.SetBool("Idle", false);
            }
        }
        else
        {
            // ✅ Fix: Stop Walk animation only when fully stopped
            animator.SetBool("Idle", true);
            animator.SetBool("Walk", false);
            animator.SetBool("Running", false);
        }
    }


    void Sprint()
    {
        float horizontal_axis = Input.GetAxisRaw("Horizontal");
        float vertical_axis = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = (playerCamera.forward * vertical_axis + playerCamera.right * horizontal_axis).normalized;
        moveDirection.y = 0f;

        bool isMoving = moveDirection.magnitude > 0.1f;
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && isMoving && onSurface;

        float moveSpeed = isSprinting ? playerSprint : playerSpeed;
        cC.Move(moveDirection * moveSpeed * Time.deltaTime);

        bool isWalking = isMoving && !isSprinting;

        // ✅ Ensure animations change correctly
        if (animator.GetBool("Running") != isSprinting)
            animator.SetBool("Running", isSprinting);

        if (animator.GetBool("Walk") != isWalking)
            animator.SetBool("Walk", isWalking);

        if (animator.GetBool("Idle") != !isMoving)
            animator.SetBool("Idle", !isMoving);
    }



    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onSurface)
        {
            velocity.y = Mathf.Sqrt(jumpRange * -2f * gravity);
            onSurface = false; // ✅ Prevent double jumping
            animator.SetTrigger("Jump");
            Debug.Log("Jump Activated!"); // ✅ Debug jump
        }

        // ✅ Fix Landing
        if (onSurface && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }
    public void playerHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        
        if (presentHealth <= 0)
        {
            PlayerDie();
        }
    }
    private void PlayerDie()
    {
        Cursor.lockState = CursorLockMode.None;
        Object.Destroy(gameObject, 1.0f);
    }



}
