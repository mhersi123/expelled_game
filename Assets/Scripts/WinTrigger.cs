using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private CanvasGroup winMenu;
    private GameObject playerHUD;
    private CanvasGroup pauseMenu;
    private PlayerStatsController playerStats;

    private void Awake()
    {
        winMenu = MenuManager.Instance.WinScreen().GetComponent<CanvasGroup>();
        if (winMenu == null)
            Debug.Log("Canvas group could not be found");

        playerHUD = MenuManager.Instance.PlayerHUD();

        pauseMenu = MenuManager.Instance.MenuScreen().GetComponent<CanvasGroup>();
        if (pauseMenu == null)
            Debug.Log("Pause menu could not be found");

        playerStats = GameManager.Instance.Player().GetComponent<PlayerStatsController>();
        if (playerStats == null)
            Debug.Log("Player stats could not be found");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playerStats.currentHealth > 0)
        {
            //winScreen.GetComponent<WinScreenToggle>().isInTriggerZone = true;
            winMenu.interactable = true;
            playerHUD.SetActive(false);
            winMenu.blocksRaycasts = true;
            winMenu.alpha = 1f;
            Time.timeScale = 0f;
            MenuManager.Instance.InteractMessage().SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
