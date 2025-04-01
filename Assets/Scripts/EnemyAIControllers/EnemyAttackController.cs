using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    public LayerMask attackLayers;
    [Tooltip("Object representing the sphere within which the enemy will attack.")]
    public GameObject attackPoint;
    public float attackRadius;
    [SerializeField] private int attackDamage = 15;

    // Start is called before the first frame update
    void Start()
    {

    }

    void AttackCheck()
    {
        Debug.Log("Enemy performing sphere check for player");
        Collider[] hitTargets = Physics.OverlapSphere(attackPoint.transform.position, attackRadius, attackLayers);
        // SphereCast
        if (hitTargets.Length > 0)
        {
            foreach (Collider col in hitTargets)
            {
                // Hit player
                Debug.Log("Enemy hit player for " + attackDamage + " damage!");

                PlayerStatsController pstats = col.gameObject.GetComponent<PlayerStatsController>();
                if (pstats == null)
                    Debug.Log("Player stats could not be found on attacked target");
                else
                    pstats.TakeDamage(attackDamage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRadius);
    }
}
