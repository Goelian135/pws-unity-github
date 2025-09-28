using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardDamage : MonoBehaviour
{
    public int damageAmount = 1; // how much damage this hazard deals

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // check if it's the player
        {
            // find the PlayerHealth component and apply damage
            PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}
