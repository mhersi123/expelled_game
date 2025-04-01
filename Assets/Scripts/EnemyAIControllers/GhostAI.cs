using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class GhostAI : MonoBehaviour, IDamageReceiver
{
    public GameObject target; // Player
    public float speed = 1.2f;
    private Animator anim;
    private GameObject enemyAttackScreen; // Used for dealing damage
    private AIState currAIState; // State Controller For AI
    private AnimationState currAnimState; // State Controller For animation
    NavMeshAgent myNavMeshAgent;
    public GameObject[] waypoints; // wander waypoints
    private int currWaypoint; // wander points index
    private PlayerStatsController playerStats;
    private bool canAttack;
    private bool canTeleport;
    private bool canShout;
    private Vector3 agentVel;
    private bool stunned;
    private float health;
    private float time;
    private float coolDown;
    private Vector3 tpPosition;
    private bool charging;

    void Start()
    {
        currAIState = AIState.Patrol;
        currAnimState = AnimationState.Idle;
        currWaypoint = -1;
        canAttack = true;
        canTeleport = false;
        canShout = true;
        time = 0f;
        coolDown = 8f;
        tpPosition = target.transform.position;
        health = 50f;
        stunned = false;
        charging = false;
    }
    void Awake()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
            Debug.Log("Animator could not be found");

        myNavMeshAgent = GetComponent<NavMeshAgent>();
        if (myNavMeshAgent == null)
            Debug.Log("NavMeshAgent could not be found");

        playerStats = target.GetComponent<PlayerStatsController>();
        if (playerStats == null)
            Debug.Log("Player stats could not be found");

        enemyAttackScreen = GameManager.Instance.EnemyAttackScreen();
        if (enemyAttackScreen == null)
            Debug.Log("Enemy Attack screen could not be found");
    }
    // Update is called once per frame
    void Update()
    {
        if (playerStats.IsDead())
        {
            currAIState = AIState.Patrol;
            AICheck();
            return;
        }
        if (currAIState == AIState.Death || stunned)
        {
            return;
        }

        time += Time.deltaTime;
        // Check if Player in NavMesh
        NavMeshPath path = new NavMeshPath();
        bool isvalid = false;
        if (target != null && NavMesh.CalculatePath(transform.position, target.transform.position, NavMesh.AllAreas, path))
        {
            isvalid = true;
            if (path.status != NavMeshPathStatus.PathComplete) isvalid = false;
        }
        if (isvalid)
        {
            canTeleport = true;
            double differenceX = target.gameObject.transform.position.x - transform.position.x;
            double differenceZ = target.gameObject.transform.position.z - transform.position.z;
            double distance = Math.Sqrt(Math.Pow(differenceX, 2) + Math.Pow(differenceZ, 2));
            if (distance > 1 && distance < 8)
            {
                if (canTeleport && coolDown < time)
                {
                    time = 0f;
                    canTeleport = false;
                    currAIState = AIState.Teleport;
                }
                else
                {
                    currAIState = AIState.Chase;
                }
            }
            else if (distance < 0.7f && canAttack)
            {
                currAIState = AIState.Attack;
            }
            else
            {
                currAIState = AIState.Patrol;
            }
        }
        else
        {
            currAIState = AIState.Patrol;
        }
        AICheck();
    }

    private void AICheck()
    {
        if (currAIState == AIState.Patrol)
        {
            if (myNavMeshAgent.destination == target.transform.position)
            {
                myNavMeshAgent.ResetPath();
            }
            if (myNavMeshAgent.remainingDistance - myNavMeshAgent.stoppingDistance == 0 && myNavMeshAgent.pathPending != true)
            {
                Wander();
            }
        }
        else if (currAIState == AIState.Chase)
        {
            Chase();
        }
        else if (currAIState == AIState.Attack)
        {
            if (canAttack)
            {
                StartCoroutine(Attack());
                StartCoroutine(AttackCooldown());
            }
        }
        else if (currAIState == AIState.Teleport)
        {
            StartCoroutine(Teleport());
        }
        else if (currAIState == AIState.Death)
        {
            Die();
        }
    }
    private void AnimationCheck()
    {
        if (currAnimState == AnimationState.Idle)
        {
            anim.SetBool("isAttacking", false);
            anim.SetBool("isWalking", false);
        }
        else if (currAnimState == AnimationState.Walk)
        {
            anim.SetBool("isAttacking", false);
            anim.SetBool("isWalking", true);
        }
        else if (currAnimState == AnimationState.Attack)
        {
            anim.SetBool("isAttacking", true);
            anim.SetBool("isWalking", false);
        }
        else if (currAnimState == AnimationState.Death)
        {
            anim.ResetTrigger("die");
            anim.SetTrigger("die");
        }
        else if (currAnimState == AnimationState.Stun)
        {
            anim.ResetTrigger("stun");
            anim.SetTrigger("stun");
        }
    }
    private void Wander()
    {
        canShout = true;
        if (currWaypoint < 0)
        {
            currWaypoint = 0;
        }
        else
        {
            currWaypoint = (currWaypoint + 1) % waypoints.Length;
        }
        myNavMeshAgent.SetDestination(waypoints[currWaypoint].transform.position);
        currAnimState = AnimationState.Walk;
        AnimationCheck();
        agentVel = myNavMeshAgent.velocity;
    }
    private void Chase()
    {
        myNavMeshAgent.SetDestination(target.transform.position);
        currAnimState = AnimationState.Walk;
        AnimationCheck();
        if (canShout)
        {
            EventManager.TriggerEvent<GhostChaseEvent, Vector3>(gameObject.transform.position);
            canShout = false;
        }
    }
    IEnumerator Teleport()
    {
        charging = true;
        anim.SetBool("charging", charging);
        EventManager.TriggerEvent<GhostChargeEvent, Vector3>(gameObject.transform.position);
        myNavMeshAgent.velocity = Vector3.zero;
        myNavMeshAgent.isStopped = true;
        currAnimState = AnimationState.Idle;
        AnimationCheck();
        yield return new WaitForSeconds(3.25f);
        tpPosition = GetWarpPoint(target.transform.position, 2f);
        yield return new WaitForSeconds(0.5f);
        myNavMeshAgent.isStopped = false;
        myNavMeshAgent.velocity = agentVel;
        EventManager.TriggerEvent<GhostWarpEvent, Vector3>(gameObject.transform.position);
        myNavMeshAgent.Warp(tpPosition);
        currAIState = AIState.Chase;
        charging = false;
        anim.SetBool("charging", charging);
        AICheck();

    }
    IEnumerator Attack()
    {

        canAttack = false;
        currAnimState = AnimationState.Attack;
        AnimationCheck();
        EventManager.TriggerEvent<GhostAttackEvent, Vector3>(gameObject.transform.position);
        //EventManager.TriggerEvent<EnemyScratchEvent, Vector3>(gameObject.transform.position);
        yield return new WaitForSeconds(1.5f); // Sync animation w/ dmg
        //playerStats.TakeDamage(16);
 

    }
  
    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        myNavMeshAgent.velocity = Vector3.zero;
        myNavMeshAgent.isStopped = true;
        currAnimState = AnimationState.Idle;
        AnimationCheck();
        // Attack, then switch to idle right after for the cooldown
        yield return new WaitForSeconds(3f);
        myNavMeshAgent.isStopped = false;
        myNavMeshAgent.velocity = agentVel;
        canAttack = true;
        canShout = true;
    }
    Vector3 GetWarpPoint(Vector3 center, float maxDistance) {
        Vector3 randomPos = UnityEngine.Random.insideUnitSphere * maxDistance + center;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas);
        return hit.position;
    }
    public void TakeDamage(float dmg)
    {
        if (!charging) 
        {
            currAnimState = AnimationState.Stun;
            AnimationCheck();
            return;
        }
        EventManager.TriggerEvent<BatHitEvent, Vector3>(gameObject.transform.position);
        stunned = true;
        canTeleport = false;
        canAttack = false;
        health -= dmg;
        if (health <= 0f)
        {
            currAIState = AIState.Death;
            AICheck();
        }
        EventManager.TriggerEvent<GhostStunEvent, Vector3>(gameObject.transform.position);
        //Debug.Log("Ghost took damage...");
        StartCoroutine(StunCooldown());
    }
    IEnumerator StunCooldown()
    {
        myNavMeshAgent.velocity = Vector3.zero;
        myNavMeshAgent.isStopped = true;
        yield return new WaitForSeconds(10f);
        myNavMeshAgent.isStopped = false;
        myNavMeshAgent.velocity = agentVel;
        canAttack = true;
        stunned = false;
        canTeleport = true;
    }
    void Die()
    {
        myNavMeshAgent.velocity = Vector3.zero;
        myNavMeshAgent.isStopped = true;
        EventManager.TriggerEvent<GhostDeathEvent, Vector3>(gameObject.transform.position);
        currAnimState = AnimationState.Death;
        AnimationCheck();
        StartCoroutine(Despawn());

    }
    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(6f);
        gameObject.SetActive(false);
    }
    enum AIState
    {
        Patrol,
        Chase,
        Teleport,
        Attack,
        Death
    }
    enum AnimationState
    {
        Idle,
        Walk,
        Attack,
        Stun,
        Death,
    }
}