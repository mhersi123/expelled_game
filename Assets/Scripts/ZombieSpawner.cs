using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ZombieSpawner : MonoBehaviour
{
    private bool walkedOn = false;
    public GameObject zombie;
    private bool pressed = false;
    private bool spawned = false;
    private bool canJump = true;
    private GameObject button;
    private ButtonTriggerZone triggerZone;
    private Unity.AI.Navigation.NavMeshSurface surface;

    void Awake()
    {

        button = GameObject.Find("basic button");
        if (button == null)
            Debug.Log("Basement Button not found");

        triggerZone = button.GetComponent<ButtonTriggerZone>();
        if (triggerZone == null)
            Debug.Log("Trigger zone not found");

        surface = GameObject.Find("HallwayNavPlane").GetComponent<Unity.AI.Navigation.NavMeshSurface>();
        if (surface == null)
            Debug.Log("Surface not found");
    }

    void Update()
    {
        pressed = triggerZone.GetPressed();

        if (!spawned)
        {
            if (pressed && walkedOn)
            {
                surface.gameObject.SetActive(true);
                //Debug.Log("Zombie Should be spawning");
                zombie.gameObject.SetActive(true);
                gameObject.SetActive(false);
                if (canJump) EventManager.TriggerEvent<JumpscareEvent, Vector3>(gameObject.transform.position);
                canJump = false;
            }
            else
            {
                surface.gameObject.SetActive(false);
                zombie.gameObject.SetActive(false);
                return;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (spawned) return;
        if (other.transform.gameObject.layer == 9)
        {
            walkedOn = true;
            //EventManager.TriggerEvent<JumpscareEvent, Vector3>(gameObject.transform.position);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (spawned) return;
        if (other.transform.gameObject.layer == 9)
        {
            walkedOn = false;
            //EventManager.TriggerEvent<FastJumpscareEvent, Vector3>(gameObject.transform.position);
        }
    }
   
}
