using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    private float filteredForwardInput = 0f;
    private float filteredStrafeInput = 0f;
    private float forwardSpeedLimit = 1f;

    public bool InputMapToCircular = true;

    public float forwardInputFilter = 5f;
    public float turnInputFilter = 5f;

    public float Forward
    {
        get;
        private set;
    }

    public float Strafe
    {
        get;
        private set;
    }

    public bool Action
    {
        get;
        private set;
    }

    public bool Jump
    {
        get;
        private set;
    }

    public bool Attack
    {
        get;
        private set;
    }

    public bool Sprint
    {
        get;
        private set;
    }

    public bool Map
    {
        get;
        private set;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //GetAxisRaw() so we can do filtering here instead of the InputManager
        float h = Input.GetAxisRaw("Horizontal");// setup h variable as our horizontal input axis
        float v = Input.GetAxisRaw("Vertical"); // setup v variables as our vertical input axis


        if (InputMapToCircular)
        {
            // make coordinates circular
            //based on http://mathproofs.blogspot.com/2005/07/mapping-square-to-circle.html
            h = h * Mathf.Sqrt(1f - 0.5f * v * v);
            v = v * Mathf.Sqrt(1f - 0.5f * h * h);
        }

        filteredForwardInput = Mathf.Clamp(Mathf.Lerp(filteredForwardInput, v,
            Time.deltaTime * forwardInputFilter), -forwardSpeedLimit, forwardSpeedLimit);

        filteredStrafeInput = Mathf.Lerp(filteredStrafeInput, h,
            Time.deltaTime * turnInputFilter);

        Forward = filteredForwardInput;
        Strafe = filteredStrafeInput;

        Jump = Input.GetButtonDown("Jump");
        Action = Input.GetButtonDown("Activate");
        Attack = Input.GetButtonDown("Fire");
        Sprint = Input.GetButton("Sprint");
        Map = Input.GetButtonDown("Map");

    }
}
