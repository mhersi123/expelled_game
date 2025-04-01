using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]

public class WinScreenToggle : MonoBehaviour
{
    public GameObject inventoryHUD;

    private CanvasGroup canvasGroup;
    private CanvasGroup pauseMenu;
    public bool isInTriggerZone = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            Debug.Log("Canvas group could not be found");
        }

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isInTriggerZone && pauseMenu.alpha == 0)
        {
            canvasGroup.interactable = true;
            inventoryHUD.SetActive(false);
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
            Time.timeScale = 0f;
            MenuManager.Instance.InteractMessage().SetActive(false);
        } else if (isInTriggerZone && pauseMenu.alpha == 1)
        {
            canvasGroup.interactable = false;
            inventoryHUD.SetActive(false);
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0f;
            Time.timeScale = 0f;
        }
    }
}
