using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{

    private PlayerMovementController pmc; 

    void Awake()
    {
        pmc = GetComponent<PlayerMovementController>();
    }

    void playFootstep(string s)
    {
        if (pmc.currentGroundLayer == 10)
        {
            EventManager.TriggerEvent<PlayerStepOutEvent, Vector3>(transform.position);
        } else
        {
            EventManager.TriggerEvent<PlayerStepEvent, Vector3>(transform.position);
        }
        Debug.Log(s + " step audio playing.");
    }

}
