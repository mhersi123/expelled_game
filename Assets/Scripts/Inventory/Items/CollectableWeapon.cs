using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableWeapon : MonoBehaviour
{
    private PlayerInventoryController ic;
    private PlayerInputController pinput;

    [SerializeField] private Vector3 weaponPosition = new Vector3(0.135f, 0.27f, 0.067f);
    [SerializeField] private Vector3 weaponRotation = new Vector3(0, 0, 88.372f);
    [SerializeField] private Vector3 weaponScale = new Vector3(2f, 2f, 2f);
    private bool isInTriggerZone = false;
    [Tooltip("This value determines the animation layer that is used when this weapon is equipped to the player. This means that all animations are unique for this weapon.")]
    [SerializeField] private int animationLayer = 3;
    public string weaponName = "unnamed";
    public bool throwable = false;

    public void OnUse()
    {
        transform.localPosition = weaponPosition;
        transform.localEulerAngles = weaponRotation;
        transform.localScale = weaponScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTriggerZone = true;
            MenuManager.Instance.InteractMessage().SetActive(true);
            ic = other.attachedRigidbody.gameObject.GetComponent<PlayerInventoryController>();
            if (ic == null)
                Debug.LogError("Could not find inventory controller on player.");
            pinput = other.attachedRigidbody.gameObject.GetComponent<PlayerInputController>();
            if (pinput == null)
                Debug.LogError("Could not find input controller on player.");
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
                Debug.Log("Player pressed Action key");
                ic.ReceiveWeapon(this);
                MenuManager.Instance.InteractMessage().SetActive(false);
                EventManager.TriggerEvent<ItemPickupEvent, Vector3>(gameObject.transform.position);
                gameObject.SetActive(false);
                isInTriggerZone = false;
            }
        }
    }

    public int GetAnimLayer()
    {
        return animationLayer;
    }
}
