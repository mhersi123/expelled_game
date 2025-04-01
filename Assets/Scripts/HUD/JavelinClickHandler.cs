using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JavelinClickHandler : MonoBehaviour
{
    private PlayerInventoryController playerInventory;
    public KeyCode key;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button == null)
            Debug.Log("Button not found");

        playerInventory = GameManager.Instance.Player().GetComponent<PlayerInventoryController>();
        if (playerInventory == null)
            Debug.Log("Could not find PlayerInventoryController from GameManager");
    }

    public void OnItemClicked()
    {
        playerInventory.EquipJavelin();
    }

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            FadeToColor(button.colors.pressedColor);
            button.onClick.Invoke();
        }
        else if (Input.GetKeyUp(key))
        {
            FadeToColor(button.colors.normalColor);
        }

    }

    void FadeToColor(Color color)
    {
        Graphic graphic = GetComponent<Graphic>();
        graphic.CrossFadeColor(color, button.colors.fadeDuration, true, true);
    }
}
