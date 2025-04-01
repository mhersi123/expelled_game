using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoorControllerKey : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private bool isInTriggerZone = false;
    private bool locked = true;
    private PlayerInventoryController ic;
    private PlayerInputController pinput;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTriggerZone = true;
            MenuManager.Instance.InteractMessage().SetActive(true);
            ic = other.attachedRigidbody.gameObject.GetComponent<PlayerInventoryController>();
            pinput = other.gameObject.GetComponent<PlayerInputController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTriggerZone = false;
            MenuManager.Instance.InteractMessage().SetActive(false);
            MenuManager.Instance.LockedKeyMessage().SetActive(false);
        }
    }

    private void Update()
    {
        if (MenuManager.Instance.InteractMessage().activeInHierarchy && isInTriggerZone && pinput != null)
        {
            if (locked)
            {
                if (pinput.Action && ic.hud.inventory.key_count > 0)
                {
                    EventManager.TriggerEvent<DoorUnlockEvent, Vector3>(gameObject.transform.position);
                    EventManager.TriggerEvent<DoorOpenEvent, Vector3>(gameObject.transform.position);
                    locked = false;
                    ic.hud.inventory.RemoveItem("Key");
                    anim.SetBool("openAction", !anim.GetBool("openAction"));
                }
                else if (pinput.Action && ic.hud.inventory.key_count == 0)
                {
                    EventManager.TriggerEvent<DoorLockedEvent, Vector3>(gameObject.transform.position);
                    MenuManager.Instance.LockedKeyMessage().SetActive(true);
                }
            } else
            {
                if (pinput.Action)
                {
                    anim.SetBool("openAction", !anim.GetBool("openAction"));
                    if (anim.GetBool("openAction"))
                    {
                        EventManager.TriggerEvent<DoorOpenEvent, Vector3>(gameObject.transform.position);
                    }
                    else
                    {
                        EventManager.TriggerEvent<DoorCloseEvent, Vector3>(gameObject.transform.position);
                    }
                }
            }

        }
        /*
        if (anim.GetBool("openAction"))
            MenuManager.Instance.InteractMessage().SetActive(false);
        */

    }
}
