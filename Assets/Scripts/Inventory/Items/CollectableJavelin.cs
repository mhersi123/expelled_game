using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableJavelin : MonoBehaviour
{
    private PlayerInventoryController ic;
    private PlayerInputController pinput;

    private Vector3 javelinPosition = new Vector3(-0.325f, 0.266f, -0.086f);
    private Vector3 javelinRotation = new Vector3(-91.7f, -45.95502f, 108.072f);
    private Vector3 javelinScale = new Vector3(2f, 2f, 2f);
    private int animationLayer = 2;

    public int GetAnimLayer()
    {
        return animationLayer;
    }

    public void OnUse()
    {
        transform.localPosition = javelinPosition;
        transform.localEulerAngles = javelinRotation;
        transform.localScale = javelinScale;
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
        //if (pinput != null && ic != null && !javelinEquipped && isInTriggerZone)
        //{
        //    if (pinput.Action)
        //    {
        //        ic.ReceiveJavelin(this);
        //        MenuManager.Instance.InteractMessage().SetActive(false);
        //        EventManager.TriggerEvent<ItemPickupEvent, Vector3>(gameObject.transform.position);
        //        gameObject.SetActive(false);
        //        javelinEquipped = true;
        //    }
        //}
    }
}
