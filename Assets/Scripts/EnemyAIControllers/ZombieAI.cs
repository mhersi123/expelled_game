using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class ZombieAI : MonoBehaviour, IDamageReceiver
{
    public GameObject target; // Player
    public float speed = 0.9f; // Speed of the zombie
    private Animator anim;

    private AIState currAIState; // State Controller For AI
    private AnimationState currAnimState; // State Controller For animation
    NavMeshAgent myNavMeshAgent;
    private PlayerStatsController playerStats;
    public GameObject[] waypoints; // wander waypoints
    private int currWaypoint; // wander points index
    private bool canAttack;
    private bool canShout;
    private Vector3 agentVel;
    private float health;
    private bool stunned;

    void Start()
    {
        currAIState = AIState.Patrol;
        currAnimState = AnimationState.Idle;
        currWaypoint = -1;
        canAttack = true;
        canShout = true;
        health = 30;
        stunned = false;
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
            double differenceX = target.gameObject.transform.position.x - transform.position.x;
            double differenceZ = target.gameObject.transform.position.z - transform.position.z;
            double distance = Math.Sqrt(Math.Pow(differenceX, 2) + Math.Pow(differenceZ, 2));
            if (distance > 1 && distance < 8 && !stunned)
            {
                currAIState = AIState.Chase;
            } 
            else if (distance < 1 && canAttack && !stunned)
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
                //myNavMeshAgent.ResetPath();
                Wander();
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
            }
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
            canShout = true;
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
        myNavMeshAgent.isStopped = false;
        canShout = true;
        if (currWaypoint < 0)
        {
            currWaypoint = 0;
        }
        else
        {
            currWaypoint = (currWaypoint + 1) % waypoints.Length;
            EventManager.TriggerEvent<ZombieIdleEvent, Vector3>(gameObject.transform.position);
        }
        myNavMeshAgent.SetDestination(waypoints[currWaypoint].transform.position);
        currAnimState = AnimationState.Walk;
        AnimationCheck();
        agentVel = myNavMeshAgent.velocity;
    }
    private void Chase()
    {
        myNavMeshAgent.isStopped = false;
        myNavMeshAgent.SetDestination(target.transform.position);
        if (canShout)
        {
            EventManager.TriggerEvent<ZombieIdleEvent, Vector3>(gameObject.transform.position);
            canShout = false;
        }
        currAnimState = AnimationState.Walk;
        AnimationCheck();
    }
    IEnumerator Attack()
    {
        canAttack = false;
        currAnimState = AnimationState.Attack;
        AnimationCheck();
        EventManager.TriggerEvent<ZombieAttackEvent, Vector3>(gameObject.transform.position);
        if (stunned) yield break;
        StartCoroutine(AttackCooldown());
    }
    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1f);
        myNavMeshAgent.velocity = Vector3.zero;
        myNavMeshAgent.isStopped = true;
        currAnimState = AnimationState.Idle;
        AnimationCheck();
        // Attack, then switch to idle right after for the cooldown
        yield return new WaitForSeconds(3f);
        myNavMeshAgent.isStopped = false;
        myNavMeshAgent.velocity = agentVel;
        canAttack = true;
    }

    public void TakeDamage(float dmg)
    {
        EventManager.TriggerEvent<BatHitEvent, Vector3>(gameObject.transform.position);
        stunned = true;
        canAttack = false;
        health -= dmg;
        if (health <= 0f)
        {
            currAIState = AIState.Death;
            AICheck();
        }
        currAnimState = AnimationState.Stun;
        AnimationCheck();
        EventManager.TriggerEvent<ZombieStunEvent, Vector3>(gameObject.transform.position);
        //Debug.Log("Zombie took damage...");
        StartCoroutine(StunCooldown());
    }
    IEnumerator StunCooldown()
    {
        myNavMeshAgent.velocity = Vector3.zero;
        myNavMeshAgent.isStopped = true;
        yield return new WaitForSeconds(1f);
        myNavMeshAgent.isStopped = false;
        myNavMeshAgent.velocity = agentVel;
        canAttack = true;
        stunned = false;
    }
    void Die()
    {
        myNavMeshAgent.velocity = Vector3.zero;
        myNavMeshAgent.isStopped = true;
        EventManager.TriggerEvent<ZombieDeathEvent, Vector3>(gameObject.transform.position);
        currAnimState = AnimationState.Death;
        AnimationCheck();
        StartCoroutine(Despawn());

    }
    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }
    enum AIState
    {
        Patrol,
        Chase,
        Attack,
        Death
    }
    enum AnimationState
    {
        Idle,
        Walk,
        Attack,
        Fall,
        Death,
        Stun
    }
}
