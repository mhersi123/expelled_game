using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AudioEventManager : MonoBehaviour
{
    public AudioClip itemPickup;
    public AudioClip doorUnlock;
    public AudioClip doorOpen;
    public AudioClip doorClose;
    public AudioClip heal;
    public AudioClip doorLocked;
    public AudioClip buttonClick;
    public AudioClip bookPull;
    public AudioClip hoopHit;
    public AudioClip bookShelf;
    public AudioClip jumpscare;

    private UnityAction<Vector3> itemPickupEventListener;
    private UnityAction<Vector3> doorUnlockEventListener;
    private UnityAction<Vector3> doorOpenEventListener;
    private UnityAction<Vector3> doorCloseEventListener;
    private UnityAction<Vector3> healEventListener;
    private UnityAction<Vector3> doorLockedListener;
    private UnityAction<Vector3> buttonClickListener;
    private UnityAction<Vector3> bookPullListener;
    private UnityAction<Vector3> hoopHitListener;
    private UnityAction<Vector3> bookShelfListener;
    private UnityAction<Vector3> jumpscareListener;

    void Awake()
    {
        itemPickupEventListener = new UnityAction<Vector3>(itemPickupHandler);
        doorUnlockEventListener = new UnityAction<Vector3>(doorUnlockHandler);
        doorOpenEventListener = new UnityAction<Vector3>(doorOpenHandler);
        doorCloseEventListener = new UnityAction<Vector3>(doorCloseHandler);
        healEventListener = new UnityAction<Vector3>(healHandler);
        doorLockedListener = new UnityAction<Vector3>(doorLockedHandler);
        buttonClickListener = new UnityAction<Vector3>(buttonClickHandler);
        bookPullListener = new UnityAction<Vector3>(bookPullHandler);
        hoopHitListener = new UnityAction<Vector3>(hoopHitHandler);
        bookShelfListener = new UnityAction<Vector3>(bookShelfHandler);
        jumpscareListener = new UnityAction<Vector3>(jumpscareHandler);
    }
    void OnEnable()
    {
        EventManager.StartListening<ItemPickupEvent, Vector3>(itemPickupEventListener);
        EventManager.StartListening<DoorUnlockEvent, Vector3>(doorUnlockEventListener);
        EventManager.StartListening<DoorOpenEvent, Vector3>(doorOpenEventListener);
        EventManager.StartListening<DoorCloseEvent, Vector3>(doorCloseEventListener);
        EventManager.StartListening<HealEvent, Vector3>(healEventListener);
        EventManager.StartListening<DoorLockedEvent, Vector3>(doorLockedListener);
        EventManager.StartListening<ButtonClickEvent, Vector3>(buttonClickListener);
        EventManager.StartListening<BookPullEvent, Vector3>(bookPullListener);
        EventManager.StartListening<HoopHitEvent, Vector3>(hoopHitListener);
        EventManager.StartListening<BookShelfEvent, Vector3>(bookShelfListener);
        EventManager.StartListening<JumpscareEvent, Vector3>(jumpscareListener);
    }

    void OnDisable()
    {
        EventManager.StopListening<ItemPickupEvent, Vector3>(itemPickupEventListener);
        EventManager.StopListening<DoorUnlockEvent, Vector3>(doorUnlockEventListener);
        EventManager.StopListening<DoorOpenEvent, Vector3>(doorOpenEventListener);
        EventManager.StopListening<DoorCloseEvent, Vector3>(doorCloseEventListener);
        EventManager.StopListening<HealEvent, Vector3>(healEventListener);
        EventManager.StopListening<DoorLockedEvent, Vector3>(doorLockedListener);
        EventManager.StopListening<ButtonClickEvent, Vector3>(buttonClickListener);
        EventManager.StopListening<BookPullEvent, Vector3>(bookPullListener);
        EventManager.StopListening<HoopHitEvent, Vector3>(hoopHitListener);
        EventManager.StopListening<BookShelfEvent, Vector3>(bookShelfListener);
        EventManager.StopListening<JumpscareEvent, Vector3>(jumpscareListener);
    }

    void itemPickupHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(itemPickup, worldPos);
    }

    void doorUnlockHandler(Vector3 worldPos)
    {
       AudioSource.PlayClipAtPoint(doorUnlock, worldPos);
    }

    void doorOpenHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(doorOpen, worldPos);
    }

    void doorCloseHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(doorClose, worldPos);
    }

    void healHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(heal, worldPos);
    }

    void doorLockedHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(doorLocked, worldPos);
    }

    void buttonClickHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(buttonClick, worldPos);
    }

    void bookPullHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(bookPull, worldPos);
    }

    void hoopHitHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(hoopHit, worldPos);
    }

    void bookShelfHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(bookShelf, worldPos);
    }
    void jumpscareHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(jumpscare, worldPos);
    }

}
