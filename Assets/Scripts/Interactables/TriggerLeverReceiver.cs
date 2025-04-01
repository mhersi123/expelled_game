using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLeverReceiver : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private int leversPulled;
    private bool check = true;
    //private bool isInTriggerZone = false;
    //public GameObject message;

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         isInTriggerZone = true;
    //         message.SetActive(true);
    //     }
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         isInTriggerZone = false;
    //         message.SetActive(false);
    //     }
    // }
    private void Start()
    {
        leversPulled = 0;
    }

    private void Update()
    {
        if (leversPulled >= 2)
        {
            anim.SetBool("hasBoth", true);
            if (check)
            {
                EventManager.TriggerEvent<BookShelfEvent, Vector3>(gameObject.transform.position);
            }
            check = false;
        }

    }

    public void pullLever() {
        leversPulled++;
    }
}
