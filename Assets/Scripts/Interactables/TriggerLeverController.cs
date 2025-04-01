using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLeverController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private bool isInTriggerZone = false;
    private bool isPulled = false;
    private PlayerInputController pinput;
    public GameObject movingBookcasesU;
    public GameObject movingBookcasesD;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPulled)
        {
            isInTriggerZone = true;
            MenuManager.Instance.InteractMessage().SetActive(true);
            pinput = other.attachedRigidbody.gameObject.GetComponent<PlayerInputController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTriggerZone = false;
            MenuManager.Instance.InteractMessage().SetActive(false);
            MenuManager.Instance.BookMessage().SetActive(false);
        }
    }


    private void Update()
    {
        if (MenuManager.Instance.InteractMessage().activeInHierarchy && isInTriggerZone && pinput != null && !isPulled) {
            if (pinput.Action)
            {
                EventManager.TriggerEvent<BookPullEvent, Vector3>(gameObject.transform.position);
                MenuManager.Instance.InteractMessage().SetActive(false);
                MenuManager.Instance.BookMessage().SetActive(true);
                isPulled = true;
                anim.SetBool("openAction", !anim.GetBool("openAction"));
                movingBookcasesU.GetComponent<TriggerLeverReceiver>().pullLever();
                movingBookcasesD.GetComponent<TriggerLeverReceiver>().pullLever();
            }
        }
    }

}
