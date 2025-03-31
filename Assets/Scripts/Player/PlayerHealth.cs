using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }
    [SerializeField] private float health = 10f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Blake has taken damage: " + damage + "Current hp = " + health);

        if (health <= 0)
        {
            health = 0;       
            Debug.Log("Blake IS DEAAD");

            // Put logic here for when the player dies
            SceneController.Instance.LoadMainMenu();
            // Maybe just respawn? whatever we want.
        }
    }
}
