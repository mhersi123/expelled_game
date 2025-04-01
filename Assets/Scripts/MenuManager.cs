using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject controlsScreen;
    [SerializeField] private GameObject interactMessage;
    [SerializeField] private GameObject lockedKeyMessage;
    [SerializeField] private GameObject lockedElectronicMessage;
    [SerializeField] private GameObject playerHUD;
    [SerializeField] private GameObject bookMessage;
    [SerializeField] private GameObject buttonMessage;
    [SerializeField] private GameObject note2;
    [SerializeField] private GameObject note3;
    [SerializeField] private GameObject note4;
    [SerializeField] private GameObject note5;

    private static MenuManager instance;

    public static MenuManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(MenuManager)) as MenuManager;

                if (!instance)
                {
                    Debug.LogError("There needs to be at least one instance of MenuManager script on an object in your scene.");
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public GameObject DeathScreen()
    {
        if (deathScreen == null)
            Debug.LogError("You need to set the deathScreen reference in the MenuManager scene obj!");
        return deathScreen;
    }

    public GameObject MenuScreen()
    {
        if (menuScreen == null)
            Debug.LogError("You need to set the menuScreen reference in the MenuManager scene obj!");
        return menuScreen;
    }

    public GameObject WinScreen()
    {
        if (winScreen == null)
            Debug.LogError("You need to set the winScreen reference in the MenuManager scene obj!");
        return winScreen;
    }

    public GameObject ControlsScreen()
    {
        if (controlsScreen == null)
            Debug.LogError("You need to set the controlsScreen reference in the MenuManager scene obj!");
        return controlsScreen;
    }

    public GameObject InteractMessage()
    {
        if (interactMessage == null)
            Debug.LogError("You need to set the interactMessage reference in the MenuManager scene obj!");
        return interactMessage;
    }

    public GameObject LockedKeyMessage()
    {
        if (lockedKeyMessage == null)
            Debug.LogError("You need to set the lockedKeyMessage reference in the MenuManager scene obj!");
        return lockedKeyMessage;
    }

    public GameObject LockedElectronicMessage()
    {
        if (lockedElectronicMessage == null)
            Debug.LogError("You need to set the lockedKeyMessage reference in the MenuManager scene obj!");
        return lockedElectronicMessage;
    }

    public GameObject PlayerHUD()
    {
        if (playerHUD == null)
            Debug.LogError("You need to set the playerHUD reference in the MenuManager scene obj!");
        return playerHUD;
    }

    public GameObject BookMessage()
    {
        if (bookMessage == null)
            Debug.LogError("You need to set the bookFirstMessage reference in the MenuManager scene obj!");
        return bookMessage;
    }

    public GameObject ButtonMessage()
    {
        if (buttonMessage == null)
            Debug.LogError("You need to set the buttonMessage reference in the MenuManager scene obj!");
        return buttonMessage;
    }

    public GameObject Note2()
    {
        if (buttonMessage == null)
            Debug.LogError("You need to set the note2 reference in the MenuManager scene obj!");
        return note2;
    }

    public GameObject Note3()
    {
        if (buttonMessage == null)
            Debug.LogError("You need to set the note3 reference in the MenuManager scene obj!");
        return note3;
    }

    public GameObject Note4()
    {
        if (buttonMessage == null)
            Debug.LogError("You need to set the note4 reference in the MenuManager scene obj!");
        return note4;
    }

    public GameObject Note5()
    {
        if (buttonMessage == null)
            Debug.LogError("You need to set the note5 reference in the MenuManager scene obj!");
        return note5;
    }

    public enum MenuState
    {

    }
}
