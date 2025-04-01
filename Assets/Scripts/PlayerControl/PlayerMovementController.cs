using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rbody;
    private PlayerInputController pinput;
    private PlayerInventoryController pinv;
    private PlayerStatsController pstats;
    private Transform cameraTransform;

    // Input values
    bool _inputAttackFired = false;
    bool _inputJumpFired = false;
    float _inputForward = 0f;
    float _inputStrafe = 0f;
    bool _inputSprint = false;

    // Movement Tuning
    public float animationSpeed = 1f;
    public float forwardMaxSpeed = 1f;
    public float backwardSpeedModifier = 0.5f;
    public float strafeMaxSpeed = 1f;
    public float rotationSpeed = 5f;
    public float sprintModifier = 2f;
    public float jumpMagnitude = 1f;
    public float jumpDistance = 1f;
    public float jumpMovementModifier = 0.25f;
    private Vector3 relVelocity;
    private bool isSprinting = false;

    // Grounded Checks
    private int groundContactCount = 0;
    [Tooltip("Useful for rough ground")]
    public float groundedOffset = -0.14f;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float groundedRadius = 0.28f;
    [Tooltip("What layers the character uses as ground")]
    public LayerMask groundLayers;
    public bool isGrounded;
    public bool isFalling = false; // Used for fall animations to block player control until complete
    public int currentGroundLayer;

    public bool GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset,
            transform.position.z);
        return Physics.CheckSphere(spherePosition, groundedRadius, groundLayers,
            QueryTriggerInteraction.Ignore);
    }

    // Attacking
    private bool isAnimationLocked;
    [SerializeField] float attackCooldown = 0f;
    private float lastAttackTime;

    // Movement Perms
    bool canJump;
    bool canAttack;
    bool canSprint;
    bool canMove;

    void Awake()
    {
        anim = GetComponent<Animator>();

        if (anim == null)
            Debug.Log("Animator could not be found");

        rbody = GetComponent<Rigidbody>();

        if (rbody == null)
            Debug.Log("Rigid body could not be found");

        pinput = GetComponent<PlayerInputController>();
        if (pinput == null)
            Debug.Log("CharacterInput could not be found");

        pinv = GetComponent<PlayerInventoryController>();
        if (pinv == null)
            Debug.Log("PlayerInventoryController could not be found!");

        pstats = GetComponent<PlayerStatsController>();
        if (pstats == null)
            Debug.Log("PlayerStatsController could not be found!");
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        isAnimationLocked = false;
    }

    // Update is called once per frame
    void Update()
    {
        _inputForward = pinput.Forward;
        _inputStrafe = pinput.Strafe;

        _inputJumpFired = _inputJumpFired || pinput.Jump;
        _inputAttackFired = _inputAttackFired || pinput.Attack;

        _inputSprint = pinput.Sprint;
    }

    private void FixedUpdate()
    {
        MovementCheck();

        relVelocity.z = Mathf.Clamp(_inputForward, -backwardSpeedModifier, 1) * forwardMaxSpeed;
        relVelocity.x = _inputStrafe * strafeMaxSpeed;

        if (_inputAttackFired & canAttack)
        {
            Attack();
        }

        if (_inputSprint && canSprint)
        {
            isSprinting = true;
            if (pstats != null)
                pstats.Sprinting();
            relVelocity.z = sprintModifier * forwardMaxSpeed;
            relVelocity.x = 0;
        }
        else
        {
            isSprinting = false;
        }

        if (!isGrounded)
        {
            relVelocity *= jumpMovementModifier;
        }

        if (_inputJumpFired && canJump)
        {
            if (pstats != null)
                pstats.Jumping();
            Jump();
        }

        Move();

        Rotate();

        anim.SetFloat("velx", _inputStrafe);
        anim.SetFloat("vely", _inputForward);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isSprinting", isSprinting);
        anim.speed = animationSpeed;

        _inputAttackFired = false;
        _inputJumpFired = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isInLayerMask(collision.transform.gameObject.layer, groundLayers))
        {
            ++groundContactCount;

            currentGroundLayer = collision.gameObject.layer;
        }


    }

    private void OnCollisionExit(Collision collision)
    {

        if (isInLayerMask(collision.transform.gameObject.layer, groundLayers))
        {
            --groundContactCount;
        }

    }

    private bool isInLayerMask(int layer, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << layer));
    }

    private void Jump()
    {
        anim.ResetTrigger("jump");
        anim.SetTrigger("jump");
        isFalling = true;
        _inputJumpFired = false;
        rbody.AddRelativeForce(Vector3.up * jumpMagnitude + jumpDistance * rbody.mass * relVelocity, ForceMode.Impulse);
    }

    private void Rotate()
    {
        // Camera based rotation
        float targetAngle = cameraTransform.eulerAngles.y;

        Quaternion rotation = Quaternion.Euler(0, targetAngle, 0);
        this.rbody.MoveRotation(Quaternion.Lerp(transform.rotation, rotation, rotationSpeed));

    }

    private void Attack()
    {
        isAnimationLocked = true;
        bool throwable = pinv.EquippedWeapon.throwable;
        string action = throwable ? "throw" : "attack";
        anim.ResetTrigger(action);
        anim.SetTrigger(action);
        _inputAttackFired = false;

        if (!throwable)
        {
            WeaponController wc = pinv.EquippedWeapon.GetComponentInChildren<WeaponController>();
            if (wc != null)
            {
                wc.EnableDamage();
            }
            else
            {
                Debug.LogError("Could not find weapon controller on weapon obj");
            }

            EventManager.TriggerEvent<PlayerAttackEvent, Vector3>(transform.position);
        }
    }

    public void EndAttack()
    {
        if (isAnimationLocked)
        {
            isAnimationLocked = false;

            lastAttackTime = Time.realtimeSinceStartup;
            
            // Is this logic better suited for a PlayerCombatController.cs?
            if (pinv.EquippedWeapon != null)
            {
                WeaponController wc = pinv.EquippedWeapon.GetComponentInChildren<WeaponController>();
                if (wc != null)
                {
                    wc.DisableDamage();
                }
            }
        }
    }

    public void EndThrow()
    {
        if (isAnimationLocked)
        {
            // Is this logic better suited for a PlayerCombatController.cs?
            ThrowableController tc = pinv.EquippedWeapon.gameObject.GetComponent<ThrowableController>();
            if (tc != null)
            {
                pinv.DropEquippedWeapon();
                tc.executeThrow();
            }
            else
            {
                Debug.LogError("Could not get throwable controller from throwable weapon");
            }
        }
    }

    public bool Sprinting()
    {
        return isSprinting;
    }

    public void Damage()
    {
        // If attack is interrupted, end it
        if (isAnimationLocked && anim.IsInTransition(0))
        {
            EndAttack();
        }

        anim.ResetTrigger("hurt");
        anim.SetTrigger("hurt");
        EventManager.TriggerEvent<PlayerHitEvent, Vector3>(this.transform.position);
    }

    public void Death()
    {
        Debug.Log("Player Died...");
        anim.SetBool("dead", true);
        enabled = false;
    }

    private void MovementCheck()
    {
        isGrounded = (groundContactCount > 0) || GroundedCheck();
        if (!isGrounded)
            isFalling = true; // set to false by landing animation

        bool isLanding = (isGrounded && isFalling); // If we are in animation to land

        canMove = !isLanding && !isAnimationLocked;
        canJump = !isSprinting && !isFalling && !isAnimationLocked && !anim.IsInTransition(0) && (pstats == null || pstats.CanJump());
        canSprint = canMove && isGrounded && (pstats == null || pstats.CanSprint());
        canAttack = canMove && !isFalling && (pinv != null && pinv.isWeaponEquipped) && (Time.realtimeSinceStartup - lastAttackTime) > attackCooldown;
    }

    private void Move()
    {
        // Programmatic Movement
        Vector3 velocity = this.transform.forward * relVelocity.z + this.transform.right * relVelocity.x;

        if (canMove)
        {
            rbody.MovePosition(rbody.position + velocity * Time.deltaTime);
        }
    }

    public bool StumbleCheck()
    {
        bool success = false;
        if (!isFalling) // Maybe do isGrounded if we want to be able to get double stomped on ?
        {
            anim.ResetTrigger("stumble");
            anim.SetTrigger("stumble");
            isAnimationLocked = true;
            success = true;
        }

        return success;
    }

    public void StopStumbling()
    {
        Debug.Log("Stopped Stubmling!");
        isAnimationLocked = false;
    }

    public void StopFalling()
    {
        isFalling = false;
    }

    private void OnDrawGizmos()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset,
        transform.position.z);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(spherePosition, groundedRadius);
    }
}
