using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerObj;
    private PlayerInputController playerInp;

    public GameObject enemyAttackScreen;

    [SerializeField] private HUD playerHUD;
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (!instance)
                {
                    Debug.LogError("There needs to be at least one instance of GameManager script on an object in your scene.");
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        instance = this;

        playerInp = playerObj.GetComponent<PlayerInputController>();
        if (!playerInp)
            Debug.LogError("GameManager could not find PlayerInputController on playerObj");
    }

    public GameObject Player()
    {
        if (playerObj == null)
            Debug.LogError("You need to set the playerObj reference in the GameManager scene obj!");
        return playerObj;
    }

    public HUD PlayerHUD()
    {
        if (playerObj == null)
            Debug.LogError("You need to set the playerHUD reference in the GameManager scene obj!");
        return playerHUD;
    }

    public GameObject EnemyAttackScreen()
    {
        return enemyAttackScreen;
    }
    public PlayerInputController PlayerInputController()
    {
        return playerInp;
    }

    public enum GameState
    {
        
    }

}
