using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;  // Maximum health of the player
    public int currentHealth; // Current health of the player

    [Header("UI Setup")]
    public GameObject healthBoxPrefab; // prefab of health box
    public Transform healthUIParent;   // parent object to hold health boxes

    private Image[] healtBoxes;

    // Start is called before the first frame update
    void Start()
    {
        SetMaxHealth(maxHealth); // Initialize health
    }

    public void SetMaxHealth(int newMax)
    {
        maxHealth = newMax;
        currentHealth = maxHealth;
        CreateHealthUI(); // Recreate the health UI
        UpdateHealthUI(); // Update the health UI
    }

    void CreateHealthUI()
    {
        foreach (Transform child in healthUIParent)
        {
            Destroy(child.gameObject); // Clear existing health boxes
        }

        // Create health boxes based on max health
        healtBoxes = new Image[maxHealth];
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject box = Instantiate(healthBoxPrefab, healthUIParent);
            healtBoxes[i] = box.GetComponent<Image>();
        }
    }
    void UpdateHealthUI()
    {
        for (int i = 0; i < healtBoxes.Length; i++)
        {
            healtBoxes[i].enabled = i < currentHealth; // Enable health boxes based on current health
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount; // Reduce current health by the damage amount
        if (currentHealth < 0)
        {
            currentHealth = 0; // Ensure health does not go below zero
        }
        UpdateHealthUI(); // Update the health UI after taking damage
    }

    public void Heal(int amount)
    {
        currentHealth += amount; // Increase current health by the heal amount
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // Ensure health does not exceed max health
        }
        UpdateHealthUI(); // Update the health UI after healing
    }


}
