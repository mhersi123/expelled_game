using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
    public HUD hud {
        get;
        private set;
    }

    public bool isWeaponEquipped = false;

    public CollectableWeapon EquippedWeapon
    {
        get;
        private set;
    }

    public CollectableWeapon BatWeapon
    {
        get;
        private set;
    }

    public CollectableWeapon JavelinWeapon
    {
        get;
        private set;
    }

    private GameObject hand;
    private string weaponEquipPath = // TODO: Super hacky way to grab this for now
        "mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder" +
        "/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/RightHandHold";
    private PlayerStatsController stats;
    private PlayerMovementController pmc;

    private void Awake()
    {
        stats = GetComponent<PlayerStatsController>();
        if (stats == null)
            Debug.Log("Stats couldn't be found");

        pmc = GetComponent<PlayerMovementController>();
        if (stats == null)
            Debug.Log("Stats couldn't be found");

        hud = GameManager.Instance.PlayerHUD();
        if (hud == null)
            Debug.Log("Player HUD couldn't be found from GameManager!");

        hand = GameManager.Instance.Player().transform.Find(weaponEquipPath).gameObject;
        if (hand == null)
            Debug.Log("Hand could not be found!");
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Stuff for new script starts here
    public void ReceiveMedkit()
    {
        hud.inventory.AddItem("Medkit");
    }

    public void ReceiveKey()
    {
        hud.inventory.AddItem("Key");
    }

    public void ReceiveWeapon(CollectableWeapon weapon)
    {
        Debug.Log("Receiving Weapon: " + weapon.weaponName);
        if (weapon.weaponName == "Bat")
        {
            if (!BatWeapon)
                BatWeapon = weapon;
        }
        else if (weapon.weaponName == "Javelin")
        {
            if (!JavelinWeapon)
                JavelinWeapon = weapon;
        }

        hud.inventory.AddItem(weapon.weaponName);
    }

    public void ConsumeMedkit()
    {
        if (hud.inventory.health_kit_count > 0 && stats.currentHealth < 100)
        {
            EventManager.TriggerEvent<HealEvent, Vector3>(gameObject.transform.position);
            stats.IncreaseHealth(20);
            hud.inventory.RemoveItem("Medkit");
        }
    }

    public void EquipBat()
    {
        UnequipWeapon();

        if (hud.inventory.bat_count > 0 && BatWeapon != null)
        {
            BatWeapon.gameObject.SetActive(true);
            Collider collider = BatWeapon.gameObject.GetComponent<Collider>();
            collider.enabled = false;
            BatWeapon.transform.parent = hand.transform;
            BatWeapon.OnUse();
            //Weapon.message.SetActive(false);
            isWeaponEquipped = true;
            EquippedWeapon = BatWeapon;
        }
    }

    public void EquipJavelin()
    {
        UnequipWeapon();

        if (hud.inventory.javelin_count > 0 && JavelinWeapon != null)
        {
            JavelinWeapon.gameObject.SetActive(true);
            Collider collider = JavelinWeapon.gameObject.GetComponent<Collider>();
            collider.enabled = false;
            JavelinWeapon.transform.parent = hand.transform;
            JavelinWeapon.OnUse();
            //Weapon.message.SetActive(false);
            isWeaponEquipped = true;
            EquippedWeapon = JavelinWeapon;
        }
    }

    public void UnequipWeapon()
    {
        if (EquippedWeapon != null && EquippedWeapon.isActiveAndEnabled)
        {
            EquippedWeapon.gameObject.SetActive(false);
            isWeaponEquipped = false;

            Collider collider = EquippedWeapon.gameObject.GetComponent<Collider>();
            collider.enabled = true;
            BatWeapon.transform.SetParent(null);
            EquippedWeapon = null;
        }
    }

    public void DropEquippedWeapon()
    {
        if (EquippedWeapon != null && EquippedWeapon.isActiveAndEnabled)
        {
            isWeaponEquipped = false;

            Collider collider = EquippedWeapon.gameObject.GetComponent<Collider>();
            collider.enabled = true;
            EquippedWeapon.transform.SetParent(null);
            hud.inventory.RemoveItem(EquippedWeapon.weaponName);
            EquippedWeapon = null;
        }
    }

    public void ConsumeKey()
    {
        if (hud.inventory.key_count > 0)
        {
            hud.inventory.RemoveItem("Key");
        }
    }
}
