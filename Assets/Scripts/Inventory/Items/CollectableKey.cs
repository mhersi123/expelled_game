using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableKey : MonoBehaviour
{
    private PlayerInventoryController ic;
    private PlayerInputController pinput;
    private bool isInTriggerZone = false;
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
            MenuManager.Instance.InteractMessage().SetActive(true);
            isInTriggerZone = true;
            ic = other.attachedRigidbody.gameObject.GetComponent<PlayerInventoryController>();
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
        if (pinput != null && ic != null && isInTriggerZone)
        {
            if (pinput.Action)
            {
                EventManager.TriggerEvent<ItemPickupEvent, Vector3>(gameObject.transform.position);
                MenuManager.Instance.InteractMessage().SetActive(false);
                ic.ReceiveKey();
                Destroy(gameObject);
            }
        }
    }
}
