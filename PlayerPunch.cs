using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    [Header("Player Punch var")]
    public Camera cam;
    public float giveDamageOf = 10f;
    public float punchingRange = 0.3f;

    //[Header("Punch Effect's")]
    //public GameObject WoodedEffect;  


    public void Punch()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, punchingRange))
        {
            Debug.Log(hitInfo.transform.name);

            ObjectToHit objectToHit = hitInfo.transform.GetComponent<ObjectToHit>();
            Zombie1 zombie1 = hitInfo.transform.GetComponent<Zombie1>();
            Zombie2 zombie2 = hitInfo.transform.GetComponent<Zombie2>();


            if (objectToHit != null)
            {
                objectToHit.ObjectHitDamage(giveDamageOf);
            }
            else if (zombie1 != null)
            {
                zombie1.zombieHitDamage(giveDamageOf);
            }
            else if (zombie2 != null)
            {
                zombie1.zombieHitDamage(giveDamageOf);
            }
        }
    }

}