using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class DeathScreenToggle : MonoBehaviour
{
    private GameObject player;
    public GameObject inventoryHUD;

    private CanvasGroup canvasGroup;
    private CanvasGroup pauseMenu;
    private PlayerStatsController playerStats;
    
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            Debug.Log("Canvas group could not be found");
        }

        player = GameManager.Instance.Player();
        if (player == null)
            Debug.Log("Player could not be found from GameManager.");

        playerStats = player.GetComponent<PlayerStatsController>();
        if (playerStats == null)
            Debug.Log("Player stats could not be found");

        pauseMenu = MenuManager.Instance.MenuScreen().GetComponent<CanvasGroup>();
        if (pauseMenu == null)
            Debug.Log("Pause menu could not be found");

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerStats.currentHealth <= 0 && pauseMenu.alpha == 0f)
        {
            canvasGroup.interactable = true;
            inventoryHUD.SetActive(false);
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
            Time.timeScale = 1f;
            MenuManager.Instance.InteractMessage().SetActive(false);
            MenuManager.Instance.LockedKeyMessage().SetActive(false);
            MenuManager.Instance.LockedElectronicMessage().SetActive(false);
            MenuManager.Instance.ButtonMessage().SetActive(false);
            MenuManager.Instance.BookMessage().SetActive(false);
            MenuManager.Instance.Note2().SetActive(false);
            MenuManager.Instance.Note3().SetActive(false);
            MenuManager.Instance.Note4().SetActive(false);
            MenuManager.Instance.Note5().SetActive(false);
        } else
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0f;
        }

    }
}
