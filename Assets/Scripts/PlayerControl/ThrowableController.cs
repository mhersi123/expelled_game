using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableController : MonoBehaviour
{
    private bool canDealDamage = false;
    [SerializeField] private float weaponDamage;
    public float throwSpeed = 10f;
    public float throwHeight = 0f;
    public LayerMask enemyLayerMask;
    public LayerMask groundLayerMask;
    List<GameObject> hasDealtDamage;
    public bool isGrounded = false;
    public bool stuckInTarget = false;
    public float stickTime = 1f;
    float lastStuckTime;

    private Rigidbody rbody;
    private Transform playerTransform;
    private Collider c;
    [SerializeField] GameObject centerOfMassMarker;
    [Tooltip("Purely the size of the gizmo to visualize the center of mass of the throwable. Use this to make it easier to position the COM")]
    [SerializeField] private float gizmoSize = 0.1f;

    public void Start()
    {
        rbody = this.GetComponent<Rigidbody>();
        if (!rbody)
            Debug.LogError("Could not find rigidbody on throwable!");

        c = this.GetComponent<Collider>();
        if (!c)
            Debug.LogError("Could not find collider on throwable!");

        playerTransform = GameManager.Instance.Player()?.transform;
        if (!playerTransform)
            Debug.LogError("Could not retrieve player transform from game manager for throwable!");

        hasDealtDamage = new List<GameObject>();

        if (centerOfMassMarker != null)
        {
            Vector3 centerOfMass = this.transform.InverseTransformPoint(centerOfMassMarker.transform.position);
            rbody.centerOfMass = centerOfMass;
        }
    }

    public void executeThrow()
    {
        canDealDamage = true;
        isGrounded = false;

        // Enable Gravity
        rbody.useGravity = true;
        rbody.isKinematic = false;
        c.isTrigger = false;

        // Face the player direction before throwing
        //transform.Rotate(new Vector3(transform.rotation.eulerAngles.x, playerTransform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));

        rbody.AddForce(gameObject.transform.forward * throwSpeed);
        //Vector3.Slerp(gameObject.transform.up, rbody.velocity.normalized, Time.deltaTime * 2);
    }

    void FixedUpdate()
    {
        if (canDealDamage && rbody.velocity != Vector3.zero)
        {
            rbody.rotation = Quaternion.LookRotation(rbody.velocity.normalized);
        }
        
        if (isGrounded && rbody.velocity == Vector3.zero)
        {
            // Wait until spear stops to disable collision
            rbody.useGravity = false;
            rbody.isKinematic = true;
            c.isTrigger = true;
        }
        
        if (stuckInTarget && Time.time > (lastStuckTime + stickTime))
        {
            Debug.Log("No longer stuck!");
            transform.SetParent(null);
            rbody.useGravity = true;
            rbody.isKinematic = false;
            stuckInTarget = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject hit = collision.collider.gameObject;
        if (IsInLayerMask(hit.layer, enemyLayerMask))
        {
            if (canDealDamage && !hasDealtDamage.Contains(hit))
            {
                Debug.Log("Throwable Hit target");

                // Stick in target
                canDealDamage = false;
                rbody.useGravity = false;
                rbody.isKinematic = true;
                this.transform.SetParent(hit.transform);
                lastStuckTime = Time.time;
                stuckInTarget = true;

                hasDealtDamage.Add(hit.transform.gameObject);

                IDamageReceiver dmgReceiver = hit.GetComponent<IDamageReceiver>();
                if (dmgReceiver != null)
                {
                    dmgReceiver.TakeDamage(weaponDamage);
                }
            }
        }
        else if (IsInLayerMask(hit.layer, groundLayerMask))
        {
            // End of throw
            isGrounded = true;
            canDealDamage = false;
            hasDealtDamage.Clear();
        }
    }

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << layer));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(centerOfMassMarker.transform.position, gizmoSize);
    }
}

