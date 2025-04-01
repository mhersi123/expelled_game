using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class CrawlerAI : MonoBehaviour, IDamageReceiver
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
    private bool canAttack;
    private bool canShout;
    private bool canCharge;
    private Vector3 agentVel;
    private float agentSpeed;
    private float health;
    private bool stunned;
    private float time;
    private float chargeCooldown;
    private Vector3 targetPos;
    Rigidbody rb;
    public float chargePower = 10f;
    public PlayerStatsController playerStats;

    void Start()
    {
        currAIState = AIState.Patrol;
        currAnimState = AnimationState.Idle;
        currWaypoint = -1;
        canAttack = true;
        canShout = true;
        health = 30f;
        stunned = false;
        time = 0f;
        canCharge = false;
        chargeCooldown = 8f;
        targetPos = Vector3.zero;
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
            canCharge = true;
            double differenceX = target.gameObject.transform.position.x - transform.position.x;
            double differenceZ = target.gameObject.transform.position.z - transform.position.z;
            double distance = Math.Sqrt(Math.Pow(differenceX, 2) + Math.Pow(differenceZ, 2));
            if (distance > 1 && distance < 8)
            {
                if (canCharge && chargeCooldown < time)
                {
                    currAIState = AIState.Charge;
                    if (targetPos == Vector3.zero) targetPos = target.transform.position;
                    time = 0f;
                    canCharge = false;
                    canShout = true;
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
        // Red flashing screen
        if (enemyAttackScreen != null)
        {
            if (enemyAttackScreen.GetComponent<Image>().color.a > 0)
            {
                var color = enemyAttackScreen.GetComponent<Image>().color;
                color.a -= 0.01f;
                enemyAttackScreen.GetComponent<Image>().color = color;
            }
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
        else if (currAIState == AIState.Charge)
        {
            StartCoroutine(Charge());
        }
        else if (currAIState == AIState.Chase)
        {
            StartCoroutine(Chase());
        }
        else if (currAIState == AIState.Attack)
        {
            if (canAttack)
            {
                Attack();
                StartCoroutine(AttackCooldown());
            }
        }
        else if (currAIState == AIState.Death)
        {
            Die();
        }
        // TODO: Functionality for AI states death and stunned/fall
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
        //EventManager.TriggerEvent<CrawlerIdleEvent, Vector3>(gameObject.transform.position);
        myNavMeshAgent.SetDestination(waypoints[currWaypoint].transform.position);
        transform.LookAt(waypoints[currWaypoint].transform.position);
        transform.Rotate(0f, 90.0f, 0f, Space.Self);
        currAnimState = AnimationState.Walk;
        AnimationCheck();
        agentVel = myNavMeshAgent.velocity;
        agentSpeed = myNavMeshAgent.speed;
    }
    IEnumerator Charge()
    {
        //myNavMeshAgent.velocity = Vector3.zero;
        currAnimState = AnimationState.Idle;
        AnimationCheck();
        myNavMeshAgent.isStopped = true;
        myNavMeshAgent.speed = 0f;
        transform.LookAt(targetPos);
        transform.Rotate(0f, 90.0f, 0f, Space.Self);
        yield return new WaitForSeconds(1f);
        anim.SetBool("run", true);
        //myNavMeshAgent.isStopped = false;
        //myNavMeshAgent.speed = 1f;
        myNavMeshAgent.isStopped = false;
        myNavMeshAgent.speed = 1.8f;
        myNavMeshAgent.speed *= 3f;
        /*if (canShout)
        {
            canShout = false;
            EventManager.TriggerEvent<CrawlerChargeEvent, Vector3>(gameObject.transform.position);
        } */
        EventManager.TriggerEvent<CrawlerChargeEvent, Vector3>(gameObject.transform.position);
        yield return new WaitForSeconds(1f);
        myNavMeshAgent.speed /= 2.5f;
        anim.SetBool("run", false);
        currAIState = AIState.Chase;
        AICheck();
        canShout = true;
    }
    IEnumerator Chase()
    {
        transform.LookAt(myNavMeshAgent.destination);
        transform.Rotate(0f, 90.0f, 0f, Space.Self);
        currAnimState = AnimationState.Walk;
        AnimationCheck();
        myNavMeshAgent.SetDestination(target.transform.position);
        yield return new WaitForSeconds(2f);
        if (canShout)
        {
            canShout = false;
            EventManager.TriggerEvent<CrawlerIdleEvent, Vector3>(gameObject.transform.position);
        }
    }
    void Attack()
    {
        canAttack = false;
        if (stunned) return;
        currAnimState = AnimationState.Attack;
        AnimationCheck();
        //yield return new WaitForSeconds(0.8f); // Sync animation w/ dmg
        EventManager.TriggerEvent<CrawlerAttackEvent, Vector3>(gameObject.transform.position);
        playerStats.TakeDamage(10);
        EventManager.TriggerEvent<LightEnemyAttackEvent, Vector3>(gameObject.transform.position);
        flashScreen();

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
    void flashScreen()
    {
        if (enemyAttackScreen != null)
        {
            var color = enemyAttackScreen.GetComponent<Image>().color;
            color.a = 0.6f;
            enemyAttackScreen.GetComponent<Image>().color = color;
        }   
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
        EventManager.TriggerEvent<CrawlerStunEvent, Vector3>(gameObject.transform.position);
        //Debug.Log("Crawler took damage...");
        StartCoroutine(StunCooldown());
    }
    IEnumerator StunCooldown()
    {
        myNavMeshAgent.velocity = Vector3.zero;
        myNavMeshAgent.isStopped = true;
        yield return new WaitForSeconds(2.5f);
        myNavMeshAgent.isStopped = false;
        myNavMeshAgent.velocity = agentVel;
        canAttack = true;
        stunned = false;
    }
    void Die()
    {
        myNavMeshAgent.velocity = Vector3.zero;
        myNavMeshAgent.speed = 0;
        myNavMeshAgent.isStopped = true;
        EventManager.TriggerEvent<CrawlerDeathEvent, Vector3>(gameObject.transform.position);
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
        Charge,
        Attack,
        Death
    }
    enum AnimationState
    {
        Idle,
        Walk,
        Run,
        Attack,
        Stun,
        Death,
    }
}
