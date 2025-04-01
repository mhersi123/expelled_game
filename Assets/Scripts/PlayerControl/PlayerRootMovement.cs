using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRootMovement : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rbody;
    private PlayerInputController cinput;
    //private StaminaBar sbar;
    //private HealthBar hbar;

    public float animationSpeed = 1f;
    public float rootMovementSpeed = 1f;
    public float rootTurnSpeed = 1f;
    public float rotationSpeed = 5f;

    public float jumpableGroundNormalMaxAngle = 45f;
    public bool closeToJumpableGround;

    bool _inputAttackFired = false;
    bool _inputJumpFired = false;
    float _inputForward = 0f;
    float _inputStrafe = 0f;

    private bool _isSprinting = false;
    private int groundContactCount = 0;

    private Transform cameraTransform;

    // Sprinting Stamina
    //public int maxStamina = 3;
    //public int staminaCooldown = 2;
    //private float stamina = 3;

    public bool IsGrounded
    {
        get
        {
            return groundContactCount > 0;
        }
    }

    void Awake()
    {
        anim = GetComponent<Animator>();

        if (anim == null)
            Debug.Log("Animator could not be found");

        rbody = GetComponent<Rigidbody>();

        if (rbody == null)
            Debug.Log("Rigid body could not be found");

        cinput = GetComponent<PlayerInputController>();
        if (cinput == null)
            Debug.Log("CharacterInput could not be found");

        //sbar = GetComponent<StaminaBar>();
        //if (sbar == null)
        //    Debug.Log("Stamina bar could not be found");

        //hbar = GetComponent<HealthBar>();
        //if (hbar == null)
        //    Debug.Log("Health bar could not be found");

    }

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        _inputForward = cinput.Forward;
        _inputStrafe = cinput.Strafe;

        _inputAttackFired = _inputAttackFired || cinput.Attack;
        _inputJumpFired = _inputJumpFired || cinput.Jump;

        _isSprinting = cinput.Sprint;
    }

    private void FixedUpdate()
    {
        if (_inputAttackFired)
        {
            anim.SetTrigger("Attack");
            _inputAttackFired = false;
        }

        if (_inputJumpFired)
        {
            anim.SetTrigger("jump");
            _inputJumpFired = false;
            anim.ResetTrigger("jump");
        }

        _inputAttackFired = false;
        _inputJumpFired = false;

        bool isGrounded = IsGrounded; //|| CharacterCommon.CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 0.1f, 1f, out closeToJumpableGround);

        anim.SetFloat("velx", _inputStrafe);
        anim.SetFloat("vely", _inputForward);
        //anim.SetBool("isFalling", !isGrounded);
        anim.SetBool("isSprinting", _isSprinting);
        anim.speed = animationSpeed;

        // Camera based rotation
        float targetAngle = cameraTransform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed);
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.gameObject.tag == "ground")
        {

            ++groundContactCount;
        }

    }

    private void OnCollisionExit(Collision collision)
    {

        if (collision.transform.gameObject.tag == "ground")
        {
            --groundContactCount;
        }

    }

    void OnAnimatorMove()
    {

        Vector3 newRootPosition;
        Quaternion newRootRotation;

        bool isGrounded = IsGrounded; //|| CharacterCommon.CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 0.1f, 1f, out closeToJumpableGround);

        if (isGrounded)
        {
            //use root motion as is if on the ground		
            newRootPosition = anim.rootPosition;
        }
        else
        {
            //Simple trick to keep model from climbing other rigidbodies that aren't the ground
            newRootPosition = new Vector3(anim.rootPosition.x, this.transform.position.y, anim.rootPosition.z);
        }

        //use rotational root motion as is
        newRootRotation = anim.rootRotation;

        // Scale the difference in position and rotation to make the character go faster or slower
        newRootPosition = Vector3.LerpUnclamped(transform.position, newRootPosition, rootMovementSpeed);
        newRootRotation = Quaternion.LerpUnclamped(transform.rotation, newRootRotation, rootTurnSpeed);

        rbody.MovePosition(newRootPosition);
        rbody.MoveRotation(newRootRotation);
    }

    //void PlayerSprint()
    //{
    //    if (sbar.stamina.BarValue > 0 && cinput.Sprint)
    //    {
    //        _isSprinting = true;
    //        sbar.stamina.BarValue = sbar.stamina.BarValue - Time.deltaTime * 40;
    //    }
    //    else
    //    {
    //        _isSprinting = false;
    //        sbar.stamina.BarValue = sbar.stamina.BarValue + Time.deltaTime * 40;

    //    }
    //}

    //void HealPlayer()
    //{
    //    hbar.health.BarValue += 20;
    //}

}
