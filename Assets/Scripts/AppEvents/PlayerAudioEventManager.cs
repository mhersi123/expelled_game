using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAudioEventManager : MonoBehaviour
{

    public AudioClip playerAttack;
    public AudioClip playerHit;
    public AudioClip batHit;
    public AudioClip footstep;
    public AudioClip footstepOut;


    private UnityAction<Vector3> playerAttackListener;
    private UnityAction<Vector3> playerHitListener;
    private UnityAction<Vector3> batHitListener;
    private UnityAction<Vector3> playerStepListener;
    private UnityAction<Vector3> playerStepOutListener;
 
    private void Awake()
    {
        playerAttackListener = new UnityAction<Vector3>(playerAttackHandler);
        playerHitListener = new UnityAction<Vector3>(playerHitHandler);
        batHitListener = new UnityAction<Vector3>(batHitHandler);
        playerStepListener = new UnityAction<Vector3>(playerStepHandler);
        playerStepOutListener = new UnityAction<Vector3>(playerStepOutHandler);
    }

    private void OnEnable()
    {
        EventManager.StartListening<PlayerAttackEvent, Vector3>(playerAttackListener);
        EventManager.StartListening<PlayerHitEvent, Vector3>(playerHitListener);
        EventManager.StartListening<BatHitEvent, Vector3>(batHitListener);
        EventManager.StartListening<PlayerStepEvent, Vector3>(playerStepListener);
        EventManager.StartListening<PlayerStepOutEvent, Vector3>(playerStepOutListener);
    }

    private void OnDisable()
    {
        EventManager.StopListening<PlayerAttackEvent, Vector3>(playerAttackListener);
        EventManager.StopListening<PlayerHitEvent, Vector3>(playerHitListener);
        EventManager.StopListening<BatHitEvent, Vector3>(batHitListener);
        EventManager.StopListening<PlayerStepEvent, Vector3>(playerStepListener);
        EventManager.StopListening<PlayerStepOutEvent, Vector3>(playerStepOutListener);
    }

    private void playerAttackHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(playerAttack, worldPos);
    }

    private void playerHitHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(playerHit, worldPos);
    }
    private void batHitHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(batHit, worldPos);
    }

    private void playerStepHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(footstep, worldPos);
    }

    private void playerStepOutHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(footstepOut, worldPos);
    }
}
