using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTriggerZone : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private bool isInTriggerZone = false;
    private bool pressed = false;
    private PlayerInputController pinput;
    public GameObject door1;
    public GameObject door2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !pressed && other.GetComponent<PlayerMovementController>().isGrounded)
        {
            isInTriggerZone = true;
            MenuManager.Instance.InteractMessage().SetActive(true);
            pinput = other.gameObject.GetComponent<PlayerInputController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTriggerZone = false;
            MenuManager.Instance.InteractMessage().SetActive(false);
            MenuManager.Instance.ButtonMessage().SetActive(false);
        }
    }

    private void Update()
    {

        if (MenuManager.Instance.InteractMessage().activeInHierarchy && isInTriggerZone && pinput != null)
        {
            if (pinput.Action)
            {
                EventManager.TriggerEvent<ButtonClickEvent, Vector3>(gameObject.transform.position);
                anim.SetBool("buttonPress", true);
                MenuManager.Instance.InteractMessage().SetActive(false);
                MenuManager.Instance.ButtonMessage().SetActive(true);
                pressed = true;
                EventManager.TriggerEvent<FastJumpscareEvent, Vector3>(gameObject.transform.position);
                door1.GetComponent<TriggerDoorControllerButton>().locked = false;
                door2.GetComponent<TriggerDoorControllerButton>().locked = false;
            }

        }

    }
    public bool GetPressed()
    {
        return pressed;
    }
}
