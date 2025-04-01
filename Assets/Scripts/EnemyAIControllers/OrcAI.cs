using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class OrcAI : MonoBehaviour, IDamageReceiver
{
    public GameObject target; // Player
    public float speed = 1.2f;
    private Animator anim;

    private AIState currAIState; // State Controller For AI
    private AnimationState currAnimState; // State Controller For animation
    private GameObject enemyAttackScreen; // Used for dealing damage
    NavMeshAgent myNavMeshAgent;
    public GameObject[] waypoints; // wander waypoints
    private int currWaypoint; // wander points index
    private PlayerStatsController playerStats;
    private bool canAttack;
    private bool canShout;
    private Vector3 agentVel;
    public GameObject escapeDoor;
    private bool stunned;
    private float health;
    private float time;
    private float coolDown;
    private bool canStomp;

    private GameObject playerCamera;
    private CameraShake cameraShake;

    void Start()
    {
        currAIState = AIState.Patrol;
        currAnimState = AnimationState.Idle;
        currWaypoint = -1;
        canAttack = true;
        canShout = true;
        stunned = false;
        health = 60f;
        time = 0f;
        coolDown = 12f;
        canStomp = false;
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

        playerCamera = GameObject.Find("PlayerCamera");
        if (playerCamera == null)
            Debug.Log("Player Camera could not be found");
        cameraShake = playerCamera.GetComponent<CameraShake>();

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
        if (currAIState == AIState.Death || stunned || currAIState == AIState.Stomp)
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
            canStomp = true;
            double differenceX = target.gameObject.transform.position.x - transform.position.x;
            double differenceZ = target.gameObject.transform.position.z - transform.position.z;
            double distance = Math.Sqrt(Math.Pow(differenceX, 2) + Math.Pow(differenceZ, 2));
            if (distance >= 1.3f && distance <= 30) // Later change so that player is safe behind the tree area
            {
                if (canStomp && coolDown + 1f < time)
                {
                    time = 0f;
                    canStomp = false;
                    currAIState = AIState.Stomp;
                }
                else
                {
                    currAIState = AIState.Chase;
                }
            }
            
            else if (distance < 1.3f && canAttack)
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
        else if (currAIState == AIState.Stomp)
        {
            StartCoroutine(Stomp());
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
        else if (currAnimState == AnimationState.Guard)
        {
            anim.ResetTrigger("guard");
            anim.SetTrigger("guard");
        }
        else if (currAnimState == AnimationState.Stomp)
        {
            anim.ResetTrigger("stomp");
            anim.SetTrigger("stomp");
        }
    }
    private void Wander()
    {
        myNavMeshAgent.ResetPath();
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
    IEnumerator Chase()
    {
        float timeArrive = 0f;
        if (canShout)
        {
            EventManager.TriggerEvent<OrcChaseEvent, Vector3>(gameObject.transform.position);
            canShout = false;
        }
        if (Vector3.Distance(transform.position, target.transform.position) > 6)
        {
            Vector3 midPoint = (target.transform.position + escapeDoor.transform.position) / 2;
            timeArrive = (transform.position - midPoint).magnitude / myNavMeshAgent.speed;
            myNavMeshAgent.SetDestination(midPoint);
            yield return new WaitForSeconds(timeArrive / 2);
            myNavMeshAgent.SetDestination(target.transform.position);
        }
        else
        {
            myNavMeshAgent.SetDestination(target.transform.position);
        }
        currAnimState = AnimationState.Walk;
        AnimationCheck();
        yield return new WaitForSeconds(0.25f + (timeArrive / 2));
    }
    void Attack()
    {
        canAttack = false;
        currAnimState = AnimationState.Attack;
        AnimationCheck();
       
        playerStats.TakeDamage(55);
        EventManager.TriggerEvent<OrcAttackEvent, Vector3>(gameObject.transform.position);
        EventManager.TriggerEvent<EnemyAttackEvent, Vector3>(gameObject.transform.position);
        flashScreen();
    }
    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1.5f);
        myNavMeshAgent.velocity = Vector3.zero;
        myNavMeshAgent.isStopped = true;
        currAnimState = AnimationState.Idle;
        AnimationCheck();
        // Attack, then switch to idle right after for the cooldown
        yield return new WaitForSeconds(.1f);
        myNavMeshAgent.isStopped = false;
        myNavMeshAgent.velocity = agentVel;
        canAttack = true;
        canShout = true;
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
        canAttack = false;
        stunned = true;
        if (dmg <= 10)
        {
            EventManager.TriggerEvent<OrcGuardEvent, Vector3>(gameObject.transform.position);
            currAnimState = AnimationState.Guard;
            AnimationCheck();
            StartCoroutine(GuardCooldown());
        } else
        {
            health -= dmg;
            if (health <= 0f)
            {
                currAIState = AIState.Death;
                AICheck();
            }
            currAnimState = AnimationState.Stun;
            AnimationCheck();
            EventManager.TriggerEvent<OrcStunEvent, Vector3>(gameObject.transform.position);
            //Debug.Log("Orc took damage...");
            StartCoroutine(StunCooldown());
        }
        
    }
    IEnumerator StunCooldown()
    {
        myNavMeshAgent.velocity = Vector3.zero;
        myNavMeshAgent.isStopped = true;
        yield return new WaitForSeconds(4f);
        myNavMeshAgent.isStopped = false;
        myNavMeshAgent.velocity = agentVel;
        canAttack = true;
        stunned = false;
    }
    IEnumerator GuardCooldown()
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
        EventManager.TriggerEvent<OrcDeathEvent, Vector3>(gameObject.transform.position);
        currAnimState = AnimationState.Death;
        AnimationCheck();
        StartCoroutine(Despawn());

    }
    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(6f);
        gameObject.SetActive(false);
    }

    void Footstep()
    {
        EventManager.TriggerEvent<OrcWalkEvent, Vector3>(gameObject.transform.position);
    }

    IEnumerator Stomp()
    {
        EventManager.TriggerEvent<OrcChargeEvent, Vector3>(gameObject.transform.position);
        myNavMeshAgent.velocity = Vector3.zero;
        myNavMeshAgent.isStopped = true;
        currAnimState = AnimationState.Idle;
        AnimationCheck();
        yield return new WaitForSeconds(1.5f);
        currAnimState = AnimationState.Stomp;
        AnimationCheck();
        currAIState = AIState.Chase;
        AICheck();
    }

    public void StompAction()
    {
        cameraShake.SetShake(true);
        EventManager.TriggerEvent<OrcStompEvent, Vector3>(gameObject.transform.position);
        StartCoroutine(AttackCooldown());
        playerStats.StumblePlayer(15f);
    }

    enum AIState
    {
        Patrol,
        Chase,
        Attack,
        Stomp,
        Death
    }
    enum AnimationState
    {
        Idle,
        Walk,
        Attack,
        Stun,
        Stomp,
        Guard,
        Death,
    }
}