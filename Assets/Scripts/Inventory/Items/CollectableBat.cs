using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBat : MonoBehaviour
{
    private PlayerInventoryController ic;
    private PlayerInputController pinput;

    private Vector3 batPosition = new Vector3(0.135f, 0.27f, 0.067f);
    private Vector3 batRotation = new Vector3(0, 0, 88.372f);
    private Vector3 batScale = new Vector3(2f, 2f, 2f);
    private int animationLayer = 3;

    public void OnUse()
    {
        transform.localPosition = batPosition;
        transform.localEulerAngles = batRotation;
        transform.localScale = batScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MenuManager.Instance.InteractMessage().SetActive(true);
            ic = other.attachedRigidbody.gameObject.GetComponent<PlayerInventoryController>();
            pinput = other.attachedRigidbody.gameObject.GetComponent<PlayerInputController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MenuManager.Instance.InteractMessage().SetActive(false);
        }
    }

    private void Update()
    {
        //if (pinput != null && ic != null && !batEquipped && isInTriggerZone)
        //{
        //    if (pinput.Action)
        //    {
        //        ic.ReceiveBat(this);
        //        MenuManager.Instance.InteractMessage().SetActive(false);
        //        EventManager.TriggerEvent<ItemPickupEvent, Vector3>(gameObject.transform.position);
        //        gameObject.SetActive(false);
        //        batEquipped = true;
        //    }
        //}
    }

    public int GetAnimLayer()
    {
        return animationLayer;
    }
}
