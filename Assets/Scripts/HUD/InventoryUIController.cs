using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUIController : MonoBehaviour
{
    public TextMeshProUGUI health_kit_count_UI;
    public TextMeshProUGUI key_count_UI;
    public TextMeshProUGUI bat_count_UI;
    public TextMeshProUGUI javelin_count_UI;


    public int health_kit_count = 0;
    public int key_count = 0;
    public int bat_count = 0;
    public int javelin_count = 0;

    public void AddItem(string itemName)
    {
        if (itemName == "Medkit")
        {
            health_kit_count += 1;
            SetCountText(health_kit_count_UI, health_kit_count);
        }

        if (itemName == "Bat")
        {
            bat_count += 1;
            SetCountText(bat_count_UI, bat_count);
        }
        
        if (itemName == "Key")
        {
            key_count += 1;
            SetCountText(key_count_UI, key_count);
        }

        if (itemName == "Javelin")
        {
            javelin_count += 1;
            SetCountText(javelin_count_UI, javelin_count);
        }
    }

    public void RemoveItem(string itemName)
    {
        if (itemName == "Medkit")
        {
            health_kit_count -= 1;
            SetCountText(health_kit_count_UI, health_kit_count);
        }

        if (itemName == "Key")
        {
            key_count -= 1;
            SetCountText(key_count_UI, key_count);
        }

        if (itemName == "Javelin")
        {
            javelin_count -= 1;
            SetCountText(javelin_count_UI, javelin_count);
        }
    }

    private void SetCountText(TextMeshProUGUI countUI, int count)
    {
        if (count < 0)
        {
            count = 0;
        }

        countUI.text = count.ToString();
    }

}
