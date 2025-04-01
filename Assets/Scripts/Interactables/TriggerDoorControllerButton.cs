using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoorControllerButton : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private bool isInTriggerZone = false;
    
    private PlayerInputController pinput;
    public bool locked = true;
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
        if (other.CompareTag("Player") && locked)
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
            MenuManager.Instance.LockedElectronicMessage().SetActive(false);
        }
    }

    private void Update()
    {

        if (locked)
        {
            if (pinput != null && pinput.Action && isInTriggerZone)
            {
                EventManager.TriggerEvent<DoorLockedEvent, Vector3>(gameObject.transform.position);
                MenuManager.Instance.LockedElectronicMessage().SetActive(true);
            }
        } else if (!locked)
        {
            anim.SetBool("openAction", true);
            
        }

    }
}
