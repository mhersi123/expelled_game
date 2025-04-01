using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsMenuToggle : MonoBehaviour
{
    private GameObject player;
    private CanvasGroup canvasGroup;
    private CanvasGroup pauseMenu;
    private PlayerStatsController playerStats;
    private PlayerInputController pinput;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            Debug.Log("Canvas group could not be found");
        }

        player = GameManager.Instance.Player();
        if (player == null)
        {
            Debug.Log("Player could not be found from GameManager.");
        }

        playerStats = player.GetComponent<PlayerStatsController>();
        if (playerStats == null)
            Debug.Log("Player stats could not be found");

        pauseMenu = MenuManager.Instance.MenuScreen().GetComponent<CanvasGroup>();
        if (pauseMenu == null)
            Debug.Log("Pause menu could not be found");

        pinput = player.GetComponent<PlayerInputController>();

    }
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

    }

    void OpenMap()
    {
        if (canvasGroup.interactable)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0f;

        }
        else
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pinput.Map && playerStats.currentHealth >= 0 && pauseMenu.alpha == 0f)
        {
            OpenMap();
        }

        if (playerStats.currentHealth <= 0 || pauseMenu.alpha == 1f)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0f;
        }
    }
       
}
