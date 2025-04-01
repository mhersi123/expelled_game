using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerNoteController : MonoBehaviour
{
    private bool isInTriggerZone = false;
    
    private PlayerInputController pinput;
    public int noteNum;
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
            MenuManager.Instance.Note2().SetActive(false);
            MenuManager.Instance.Note3().SetActive(false);
            MenuManager.Instance.Note4().SetActive(false);
            MenuManager.Instance.Note5().SetActive(false);
        }
    }

    private void Update()
    {

        if (pinput != null && pinput.Action && isInTriggerZone && noteNum == 2)
            {
                //EventManager.TriggerEvent<DoorLockedEvent, Vector3>(gameObject.transform.position);
                MenuManager.Instance.Note2().SetActive(true);
                MenuManager.Instance.InteractMessage().SetActive(false);
        } else if (pinput != null && pinput.Action && isInTriggerZone && noteNum == 3) {
                MenuManager.Instance.Note3().SetActive(true);
                MenuManager.Instance.InteractMessage().SetActive(false);
        } else if (pinput != null && pinput.Action && isInTriggerZone && noteNum == 4) {
                MenuManager.Instance.Note4().SetActive(true);
                MenuManager.Instance.InteractMessage().SetActive(false);
        } else if (pinput != null && pinput.Action && isInTriggerZone && noteNum == 5) {
                MenuManager.Instance.Note5().SetActive(true);
                MenuManager.Instance.InteractMessage().SetActive(false);
        }
    }
}
