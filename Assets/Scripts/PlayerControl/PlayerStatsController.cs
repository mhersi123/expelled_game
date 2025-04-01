using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsController : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;

    public float maxStamina = 100;
    public float currentStamina;

    private HUD hud;

    private PlayerInputController input;
    private PlayerMovementController movement;
    private GameObject playerDamageScreen; // Red flash for damage

    [Tooltip("Cooldown in seconds before player can regenerate Stamina")]
    public float coolDownCounter = 2;
    private bool coolDown = false;

    private bool dead = false;


    private void Awake()
    {
        input = GetComponent<PlayerInputController>();

        if (input == null)
            Debug.Log("Input controller could not be found");
        movement = GetComponent<PlayerMovementController>();

        if (movement == null)
            Debug.Log("Movement controller could not be found");

        hud = GameManager.Instance.PlayerHUD();
        if (hud == null)
            Debug.Log("Player HUD could not be found from GameManager");

        playerDamageScreen = GameManager.Instance.EnemyAttackScreen();
        if (playerDamageScreen == null)
            Debug.Log("Player Damage screen could not be found");
    }

    // Start is called before the first frame update
    void Start()
    {

        currentHealth = maxHealth;
        hud.healthBar.SetMaxHealth(maxHealth);

        currentStamina = maxStamina;
        hud.staminaBar.SetMaxStamina(maxStamina);

    }

    // Update is called once per frame
    void Update()
    {
        DamageScreenReset();
        CoolDown();
        StaminaRegen();

        /*
        // Debug Controls (Remove Later)
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(10);
            DecreaseStamina(10);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            IncreaseHealth(10);
            IncreaseStamina(10);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            StumblePlayer(10);
        }
        */
        
    }

    public void TakeDamage(float healthAmount)
    {
        if (!dead)
        {
            DecreaseHealth(healthAmount);
            movement.Damage(); // Damage animations
            DamageScreenFlash();
        }
    }

    public void StumblePlayer(float healthAmount)
    {
        if (!dead)
        {
            if (movement.StumbleCheck())
            {
                DecreaseHealth(healthAmount);
                DamageScreenFlash();
            }
        }
    }

    public void DecreaseHealth(float HealthAmount)
    {
        currentHealth -= HealthAmount;
        if (HealthAmount == 8f) EventManager.TriggerEvent<EnemyAttackEvent, Vector3>(gameObject.transform.position);
        if (HealthAmount == 16f) EventManager.TriggerEvent<EnemyScratchEvent, Vector3>(gameObject.transform.position);
        ValuesCheck();
        hud.healthBar.SetHealth(currentHealth);
    }

    public void DecreaseStamina(float StaminaAmount)
    {
        currentStamina -= StaminaAmount;
        ValuesCheck();
        hud.staminaBar.SetStamina(currentStamina);
    }

    public void IncreaseHealth(float HealthAmount)
    {
        currentHealth += HealthAmount;
        ValuesCheck();
        hud.healthBar.SetHealth(currentHealth);
    }

    public void IncreaseStamina(float StaminaAmount)
    {
        currentStamina += StaminaAmount;
        ValuesCheck();
        hud.staminaBar.SetStamina(currentStamina);
    }

    public void Sprinting()
    {
        //This is the case if you pressed sprint and you have stamina
        if (currentStamina > 0)
        {
            currentStamina = currentStamina - Time.deltaTime * 40;
            hud.staminaBar.SetStamina(currentStamina - Time.deltaTime * 40);
        }

        //Case if you pressed sprint and you ran out of stamina
        if (currentStamina <= 0)
        {
            coolDown = true;
        }
    }

    public void Jumping()
    {
        //currentStamina = currentStamina - Time.deltaTime * 40;
        //This is the case if you pressed jump and you have stamina
        if (currentStamina > 0)
        {
            currentStamina = currentStamina - 20;
            hud.staminaBar.SetStamina(currentStamina);
        }

        //Case if you pressed sprint and you ran out of stamina
        if (currentStamina <= 0)
        {
            coolDown = true;
        }
    }
    public bool CanSprint()
    {
        return !coolDown && (currentStamina > 0);
    }

    public bool CanJump()
    {
        return !coolDown && (currentStamina >= 20);
    }


    private void StaminaRegen()
    {
        if (currentStamina < 100 && !coolDown && currentHealth > 0)
        {
            currentStamina = currentStamina + Time.deltaTime * 10;
            if (currentStamina > 100)
            {
                currentStamina = 100;
            }

            hud.staminaBar.SetStamina(currentStamina + Time.deltaTime * 10);
        }
    }

    private void ValuesCheck()
    {
        if (currentHealth > 100)
        {
            currentHealth = 100;
            hud.healthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {

            movement.Death();
            currentHealth = 0;
            hud.healthBar.SetHealth(currentHealth);
            dead = true;
        }

        if (currentStamina > 100)
        {
            currentStamina = 100;
            hud.staminaBar.SetStamina(currentStamina);
        }

        if (currentStamina < 0)
        {
            currentStamina = 0;
            hud.staminaBar.SetStamina(currentStamina);
        }
       
    }

    private void CoolDown()
    {
        //Cooldown timer that activates when you have 0 stamina
        if (coolDown == true)
        {
            hud.staminaBar.NoStamina();

            coolDownCounter -= Time.deltaTime;
            if (coolDownCounter <= 0)
            {
                hud.staminaBar.hasStamina();

                coolDownCounter = 2;
                coolDown = false;
            }
        }
    }

    void DamageScreenFlash()
    {
        if (playerDamageScreen != null)
        {
            var color = playerDamageScreen.GetComponent<Image>().color;
            color.a = 0.6f;
            playerDamageScreen.GetComponent<Image>().color = color;
        }
    }

    public void DamageScreenReset()
    {
        // Red flashing screen
        if (playerDamageScreen != null)
        {
            if (playerDamageScreen.GetComponent<Image>().color.a > 0)
            {
                var color = playerDamageScreen.GetComponent<Image>().color;
                color.a -= 0.01f;
                playerDamageScreen.GetComponent<Image>().color = color;
            }
        }
    }
    public bool IsDead()
    {
        return dead;
    }
}
