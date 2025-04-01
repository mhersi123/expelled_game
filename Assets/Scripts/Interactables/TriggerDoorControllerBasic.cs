using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoorControllerBasic : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private bool isInTriggerZone = false;
    private PlayerInputController pinput;

    private void Start()
    {
        pinput = GameManager.Instance.Player().GetComponent<PlayerInputController>();
        if (!pinput)
        {
            Debug.LogError("Could not get player input controller from game manager!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTriggerZone = true;
            MenuManager.Instance.InteractMessage().SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTriggerZone = false;
            MenuManager.Instance.InteractMessage().SetActive(false);
        }
    }

    private void Update()
    {
    
        if (MenuManager.Instance.InteractMessage() && isInTriggerZone && pinput != null)
        {
            if (pinput.Action)
            {
                anim.SetBool("openAction", !anim.GetBool("openAction"));
                if (anim.GetBool("openAction"))
                {
                    EventManager.TriggerEvent<DoorOpenEvent, Vector3>(gameObject.transform.position);
                } else
                {
                    EventManager.TriggerEvent<DoorCloseEvent, Vector3>(gameObject.transform.position);
                }
                
            }
        }
      
    }
}
