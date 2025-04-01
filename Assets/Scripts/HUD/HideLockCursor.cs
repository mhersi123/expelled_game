using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideLockCursor : MonoBehaviour
{
    // Start is called before the first frame update

    private CanvasGroup pauseMenu;
    private CanvasGroup deathMenu;
    private CanvasGroup winMenu;

    private void Awake()
    {
        winMenu = MenuManager.Instance.WinScreen().GetComponent<CanvasGroup>();
        if (winMenu == null)
            Debug.Log("Canvas group could not be found");

        pauseMenu = MenuManager.Instance.MenuScreen().GetComponent<CanvasGroup>();
        if (pauseMenu == null)
            Debug.Log("Pause menu could not be found");

        deathMenu = MenuManager.Instance.DeathScreen().GetComponent<CanvasGroup>();
        if (deathMenu == null)
            Debug.Log("Pause menu could not be found");

    }

    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {

        //Testing purposes
        /*
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        */

        if (pauseMenu.alpha == 1f || deathMenu.alpha == 1f || winMenu.alpha == 1f) 
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
