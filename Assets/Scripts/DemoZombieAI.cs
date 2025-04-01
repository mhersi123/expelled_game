using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
public class DemoZombieAI : MonoBehaviour
{

    // Target must be the player, so that the zombie is chasing them
    public GameObject target;
    public float speed = 0.8f;
    private Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
        double differenceX = target.gameObject.transform.position.x - transform.position.x;
        double differenceZ = target.gameObject.transform.position.z - transform.position.z;
        double distance = Math.Sqrt(Math.Pow(differenceX, 2) + Math.Pow(differenceZ, 2));
        if (distance > 0.5 && distance < 14) 
        {
            anim.SetFloat("vely", .06f);
            anim.SetBool("attack", false);
            transform.LookAt(target.gameObject.transform);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        else
        {
            if (distance <= 0.5)
            {
                anim.SetBool("attack", true);
            }
            anim.SetFloat("vely", .03f);
        }
        }
    }
}
