using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthClickHandler : MonoBehaviour
{
    private PlayerInventoryController playerInventory;
    public KeyCode key;
    private Button button;

    private void Awake()
    {
        playerInventory = GameManager.Instance.Player().GetComponent<PlayerInventoryController>();
        if (playerInventory == null)
            Debug.Log("Could not find PlayerInventoryController from GameManager");

        button = GetComponent<Button>();
        if (button == null)
            Debug.Log("Button not found");
    }

    public void OnItemClicked()
    {
        playerInventory.ConsumeMedkit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            FadeToColor(button.colors.pressedColor);
            button.onClick.Invoke();
        } else if (Input.GetKeyUp(key)) {
            FadeToColor(button.colors.normalColor);
        }

    }

    void FadeToColor(Color color)
    {
        Graphic graphic = GetComponent<Graphic>();
        graphic.CrossFadeColor(color, button.colors.fadeDuration, true, true);
    }


}
