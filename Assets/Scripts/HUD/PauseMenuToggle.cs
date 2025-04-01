using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PauseMenuToggle : MonoBehaviour
{
    public GameObject inventoryHUD;
    private CanvasGroup canvasGroup;
    private CanvasGroup winMenu;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            Debug.Log("Canvas group could not be found");
        }

        winMenu = MenuManager.Instance.WinScreen().GetComponent<CanvasGroup>();
        if (winMenu == null)
            Debug.Log("Canvas group could not be found");
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && winMenu.alpha == 0)
        {
            if (canvasGroup.interactable)
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.alpha = 0f;
                Time.timeScale = 1f;
                inventoryHUD.SetActive(true);

            } 
            else
            {
                MenuManager.Instance.InteractMessage().SetActive(false);
                MenuManager.Instance.LockedKeyMessage().SetActive(false);
                MenuManager.Instance.LockedElectronicMessage().SetActive(false);
                MenuManager.Instance.ButtonMessage().SetActive(false);
                MenuManager.Instance.BookMessage().SetActive(false);
                MenuManager.Instance.Note2().SetActive(false);
                MenuManager.Instance.Note3().SetActive(false);
                MenuManager.Instance.Note4().SetActive(false);
                MenuManager.Instance.Note5().SetActive(false);
                inventoryHUD.SetActive(false);
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                canvasGroup.alpha = 1f;
                Time.timeScale = 0f;
            }
        }
    }
}
