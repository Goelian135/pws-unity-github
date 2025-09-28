using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthIncreaseAmount = 1; // Amount of health to increase    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.SetMaxHealth(playerHealth.maxHealth + healthIncreaseAmount);
            Destroy(gameObject); // Destroy the pickup after use
        }
    }
}
