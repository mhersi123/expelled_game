using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public GameObject noStaminaFill;

    public void SetStamina(float stamina)
    {
        slider.value = stamina;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetMaxStamina(float stamina)
    {
        slider.maxValue = stamina;
        slider.value = stamina;

        fill.color = gradient.Evaluate(1f);
    }

    public void NoStamina()
    {
        noStaminaFill.SetActive(true);
    }

    public void hasStamina()
    {
        noStaminaFill.SetActive(false);
    }
}
