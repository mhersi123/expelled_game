using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAudioEventManager : MonoBehaviour
{
    public AudioClip enemyAttack;
    public AudioClip enemyScratch;
    public AudioClip enemyLightAttack;

    public AudioClip zombieIdle;
    public AudioClip zombieAttack;
    public AudioClip zombieChase;
    public AudioClip zombieStun;
    public AudioClip zombieDeath;

    public AudioClip zombieScream;
    public AudioClip fastJumpscare;

    public AudioClip crawlerIdle;
    public AudioClip crawlerCharge;
    public AudioClip crawlerAttack;
    public AudioClip crawlerStun;
    public AudioClip crawlerDeath;

    public AudioClip ghostChase;
    public AudioClip ghostAttack;
    public AudioClip ghostWarp;
    public AudioClip ghostStun;
    public AudioClip ghostDeath;
    public AudioClip ghostCharge;

    public AudioClip orcChase;
    public AudioClip orcAttack;
    public AudioClip orcStun;
    public AudioClip orcDeath;
    public AudioClip orcGuard;
    public AudioClip orcWalk;
    public AudioClip orcCharge;
    public AudioClip orcStomp;

    private UnityAction<Vector3> enemyAttackListener;
    private UnityAction<Vector3> enemyScratchListener;
    private UnityAction<Vector3> enemyLightAttackListener;

    private UnityAction<Vector3> zombieChaseListener;
    private UnityAction<Vector3> zombieIdleListener;
    private UnityAction<Vector3> zombieAttackListener;
    private UnityAction<Vector3> zombieStunListener;
    private UnityAction<Vector3> zombieDeathListener;
    private UnityAction<Vector3> zombieScreamListener;
    private UnityAction<Vector3> fastJumpscareListener;

    private UnityAction<Vector3> crawlerIdleListener;
    private UnityAction<Vector3> crawlerAttackListener;
    private UnityAction<Vector3> crawlerChargeListener;
    private UnityAction<Vector3> crawlerStunListener;
    private UnityAction<Vector3> crawlerDeathListener;

    private UnityAction<Vector3> ghostChaseListener;
    private UnityAction<Vector3> ghostWarpListener;
    private UnityAction<Vector3> ghostAttackListener;
    private UnityAction<Vector3> ghostStunListener;
    private UnityAction<Vector3> ghostDeathListener;
    private UnityAction<Vector3> ghostChargeListener;

    private UnityAction<Vector3> orcChaseListener;
    private UnityAction<Vector3> orcAttackListener;
    private UnityAction<Vector3> orcStunListener;
    private UnityAction<Vector3> orcDeathListener;
    private UnityAction<Vector3> orcGuardListener;
    private UnityAction<Vector3> orcWalkListener;
    private UnityAction<Vector3> orcChargeListener;
    private UnityAction<Vector3> orcStompListener;

    void Awake()
    {
        enemyAttackListener = new UnityAction<Vector3>(enemyAttackHandler);
        enemyScratchListener = new UnityAction<Vector3>(enemyScratchHandler);
        enemyLightAttackListener = new UnityAction<Vector3>(enemyLightAttackHandler);

        zombieAttackListener = new UnityAction<Vector3>(zombieAttackHandler);
        zombieIdleListener = new UnityAction<Vector3>(zombieIdleHandler);
        zombieChaseListener = new UnityAction<Vector3>(zombieChaseHandler);
        zombieStunListener = new UnityAction<Vector3>(zombieStunHandler);
        zombieDeathListener = new UnityAction<Vector3>(zombieDeathHandler);
        zombieScreamListener = new UnityAction<Vector3>(zombieScreamHandler);
        fastJumpscareListener = new UnityAction<Vector3>(fastJumpscareHandler);

        crawlerChargeListener = new UnityAction<Vector3>(crawlerChargeHandler);
        crawlerIdleListener = new UnityAction<Vector3>(crawlerIdleHandler);
        crawlerAttackListener = new UnityAction<Vector3>(crawlerAttackHandler);
        crawlerStunListener = new UnityAction<Vector3>(crawlerStunHandler);
        crawlerDeathListener = new UnityAction<Vector3>(crawlerDeathHandler);

        ghostChaseListener = new UnityAction<Vector3>(ghostChaseHandler);
        ghostWarpListener = new UnityAction<Vector3>(ghostWarpHandler);
        ghostAttackListener = new UnityAction<Vector3>(ghostAttackHandler);
        ghostStunListener = new UnityAction<Vector3>(ghostStunHandler);
        ghostDeathListener = new UnityAction<Vector3>(ghostDeathHandler);
        ghostChargeListener = new UnityAction<Vector3>(ghostChargeHandler);

        orcChaseListener = new UnityAction<Vector3>(orcChaseHandler);
        orcAttackListener = new UnityAction<Vector3>(orcAttackHandler);
        orcStunListener = new UnityAction<Vector3>(orcStunHandler);
        orcDeathListener = new UnityAction<Vector3>(orcDeathHandler);
        orcGuardListener = new UnityAction<Vector3>(orcGuardHandler);
        orcWalkListener = new UnityAction<Vector3>(orcWalkHandler);
        orcChargeListener = new UnityAction<Vector3>(orcChargeHandler);
        orcStompListener = new UnityAction<Vector3>(orcStompHandler);
    }
    void OnEnable()
    {
        EventManager.StartListening<EnemyAttackEvent, Vector3>(enemyAttackListener);
        EventManager.StartListening<EnemyScratchEvent, Vector3>(enemyScratchListener);
        EventManager.StartListening<LightEnemyAttackEvent, Vector3>(enemyLightAttackListener);

        EventManager.StartListening<ZombieAttackEvent, Vector3>(zombieAttackListener);
        EventManager.StartListening<ZombieIdleEvent, Vector3>(zombieIdleListener);
        EventManager.StartListening<ZombieChaseEvent, Vector3>(zombieChaseListener);
        EventManager.StartListening<ZombieStunEvent, Vector3>(zombieStunListener);
        EventManager.StartListening<ZombieDeathEvent, Vector3>(zombieDeathListener);
        EventManager.StartListening<ZombieScreamEvent, Vector3>(zombieScreamListener);
        EventManager.StartListening<FastJumpscareEvent, Vector3>(fastJumpscareListener);

        EventManager.StartListening<CrawlerIdleEvent, Vector3>(crawlerIdleListener);
        EventManager.StartListening<CrawlerChargeEvent, Vector3>(crawlerChargeListener);
        EventManager.StartListening<CrawlerAttackEvent, Vector3>(crawlerAttackListener);
        EventManager.StartListening<CrawlerStunEvent, Vector3>(crawlerStunListener);
        EventManager.StartListening<CrawlerDeathEvent, Vector3>(crawlerDeathListener);

        EventManager.StartListening<GhostChaseEvent, Vector3>(ghostChaseListener);
        EventManager.StartListening<GhostWarpEvent, Vector3>(ghostWarpListener);
        EventManager.StartListening<GhostAttackEvent, Vector3>(ghostAttackListener);
        EventManager.StartListening<GhostStunEvent, Vector3>(ghostStunListener);
        EventManager.StartListening<GhostDeathEvent, Vector3>(ghostDeathListener);
        EventManager.StartListening<GhostChargeEvent, Vector3>(ghostChargeListener);

        EventManager.StartListening<OrcChaseEvent, Vector3>(orcChaseListener);
        EventManager.StartListening<OrcAttackEvent, Vector3>(orcAttackListener);
        EventManager.StartListening<OrcStunEvent, Vector3>(orcStunListener);
        EventManager.StartListening<OrcDeathEvent, Vector3>(orcDeathListener);
        EventManager.StartListening<OrcGuardEvent, Vector3>(orcGuardListener);
        EventManager.StartListening<OrcWalkEvent, Vector3>(orcWalkListener);
        EventManager.StartListening<OrcChargeEvent, Vector3>(orcChargeListener);
        EventManager.StartListening<OrcStompEvent, Vector3>(orcStompListener);
    }

    void OnDisable()
    {
        EventManager.StopListening<EnemyAttackEvent, Vector3>(enemyAttackListener);
        EventManager.StopListening<EnemyScratchEvent, Vector3>(enemyScratchListener);
        EventManager.StopListening<LightEnemyAttackEvent, Vector3>(enemyLightAttackListener);

        EventManager.StopListening<ZombieAttackEvent, Vector3>(zombieAttackListener);
        EventManager.StopListening<ZombieIdleEvent, Vector3>(zombieIdleListener);
        EventManager.StopListening<ZombieChaseEvent, Vector3>(zombieChaseListener);
        EventManager.StopListening<ZombieStunEvent, Vector3>(zombieStunListener);
        EventManager.StopListening<ZombieDeathEvent, Vector3>(zombieDeathListener);
        EventManager.StopListening<ZombieScreamEvent, Vector3>(zombieScreamListener);
        EventManager.StopListening<FastJumpscareEvent, Vector3>(fastJumpscareListener);

        EventManager.StopListening<CrawlerIdleEvent, Vector3>(crawlerIdleListener);
        EventManager.StopListening<CrawlerChargeEvent, Vector3>(crawlerChargeListener);
        EventManager.StopListening<CrawlerAttackEvent, Vector3>(crawlerAttackListener);
        EventManager.StopListening<CrawlerStunEvent, Vector3>(crawlerStunListener);
        EventManager.StopListening<CrawlerDeathEvent, Vector3>(crawlerDeathListener);

        EventManager.StopListening<GhostChaseEvent, Vector3>(ghostChaseListener);
        EventManager.StopListening<GhostWarpEvent, Vector3>(ghostWarpListener);
        EventManager.StopListening<GhostAttackEvent, Vector3>(ghostAttackListener);
        EventManager.StopListening<GhostStunEvent, Vector3>(ghostStunListener);
        EventManager.StopListening<GhostDeathEvent, Vector3>(ghostDeathListener);
        EventManager.StopListening<GhostChargeEvent, Vector3>(ghostChargeListener);

        EventManager.StopListening<OrcChaseEvent, Vector3>(orcChaseListener);
        EventManager.StopListening<OrcAttackEvent, Vector3>(orcAttackListener);
        EventManager.StopListening<OrcStunEvent, Vector3>(orcStunListener);
        EventManager.StopListening<OrcDeathEvent, Vector3>(orcDeathListener);
        EventManager.StopListening<OrcGuardEvent, Vector3>(orcGuardListener);
        EventManager.StopListening<OrcWalkEvent, Vector3>(orcWalkListener);
        EventManager.StopListening<OrcChargeEvent, Vector3>(orcChargeListener);
        EventManager.StopListening<OrcStompEvent, Vector3>(orcStompListener);
    }
    void enemyAttackHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(enemyAttack, worldPos);
    }
    void enemyScratchHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(enemyScratch, worldPos);
    }
    void enemyLightAttackHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(enemyLightAttack, worldPos);
    }
    void zombieAttackHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(zombieAttack, worldPos);
    }
    void zombieIdleHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(zombieIdle, worldPos);
    }
    void zombieChaseHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(zombieChase, worldPos);
    }
    void zombieStunHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(zombieStun, worldPos);
    }
    void zombieDeathHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(zombieDeath, worldPos);
    }
    void zombieScreamHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(zombieScream, worldPos);
    }
    void fastJumpscareHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(fastJumpscare, worldPos);
    }
    void crawlerChargeHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(crawlerCharge, worldPos);
    }
    void crawlerIdleHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(crawlerIdle, worldPos);
    }
    void crawlerAttackHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(crawlerAttack, worldPos);
    }
    void crawlerStunHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(crawlerStun, worldPos);
    }
    void crawlerDeathHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(crawlerDeath, worldPos);
    }
    void ghostChaseHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(ghostChase, worldPos);
    }
    void ghostAttackHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(ghostAttack, worldPos);
    }
    void ghostWarpHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(ghostWarp, worldPos);
    }
    void ghostStunHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(ghostStun, worldPos);
    }
    void ghostDeathHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(ghostDeath, worldPos);
    }
    void ghostChargeHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(ghostCharge, worldPos);
    }
    void orcChaseHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(orcChase, worldPos);
    }
    void orcAttackHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(orcAttack, worldPos);
    }
    void orcStunHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(orcStun, worldPos);
    }
    void orcDeathHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(orcDeath, worldPos);
    }
    void orcGuardHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(orcGuard, worldPos);
    }
    void orcWalkHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(orcWalk, worldPos);
    }
    void orcChargeHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(orcCharge, worldPos);
    }
    void orcStompHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(orcStomp, worldPos);
    }
}