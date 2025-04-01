using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // Inspired by https://www.youtube.com/watch?v=uGFzWM1sJjU 
    public bool canDealDamage;
    List<GameObject> hasDealtDamage;
    public LayerMask enemyLayerMask;

    [SerializeField] float weaponLength;
    [SerializeField] float weaponDamage;
    [SerializeField] float knockbackForce;

    // Start is called before the first frame update
    void Start()
    {
        hasDealtDamage = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canDealDamage)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, enemyLayerMask))
            {
                if (!hasDealtDamage.Contains(hit.transform.gameObject))
                {
                    //Debug.Log("Hit target");
                    hasDealtDamage.Add(hit.transform.gameObject);

                    // Apply knockback (maybe do this on zombie side later)
                    if (hit.rigidbody)
                    {
                        hit.rigidbody.AddForce((-hit.normal - hit.transform.forward) * knockbackForce, ForceMode.Force);
                    }

                    IDamageReceiver dmgReceiver = hit.transform.gameObject.GetComponent<IDamageReceiver>();
                    if (dmgReceiver != null)
                    {
                        dmgReceiver.TakeDamage(weaponDamage);
                        //Debug.Log("Target has been hit");
                    }
                }
            }
        }
    }

    public void EnableDamage()
    {
        canDealDamage = true;
        hasDealtDamage.Clear();
    }

    public void DisableDamage()
    {
        canDealDamage = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLength);
    }
}
