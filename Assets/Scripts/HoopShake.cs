using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopShake : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private bool isInTriggerZone = false;



    private void OnTriggerEnter(Collider other)
    {
        //REMOVE COMPARE PLAYER TAG ONCE CRAWLER IS COMPLETE
        if (other.CompareTag("Enemy"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            float speed = rb.velocity.magnitude;
            if (speed > .5) 
            {
                isInTriggerZone = true;
                anim.SetBool("openAction", true);
                EventManager.TriggerEvent<HoopHitEvent, Vector3>(gameObject.transform.position);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            isInTriggerZone = false;
            anim.SetBool("openAction", false);
        }
    }


    private void Update()
    {
        if (isInTriggerZone) {
                

        }
    }

}
