using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiflePickup : MonoBehaviour
{
    [Header("Rifle's")]
    public GameObject PlayerRifle;
    public GameObject PickupRifle;
    public PlayerPunch playerPunch;


    [Header("Rifle Assign Things")]
    public PlayerScript player;
    private float radius = 2.5f;
    public Animator animator;
    private float nextTimeToPunch = 0f;
    public float punchCharge = 15f;

    private void Awake()
    {
        PlayerRifle.SetActive(false);
    }

    private void Update()
    {
         if(Input.GetButton("Fire1") && Time.time >= nextTimeToPunch)
        {
            animator.SetBool("Punch", true);
            animator.SetBool("Idle", false);

            nextTimeToPunch = Time.time + 1f/punchCharge;

            playerPunch.Punch();


        }
        else
        {
            animator.SetBool("Punch", false);
            animator.SetBool("Idle", true);

        }
        if(Vector3.Distance(transform.position, player.transform.position) < radius) 
        {
            if (Input.GetKeyDown("f"))
            {
                PlayerRifle.SetActive(true);

            }
        }
    }
}
