using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int InitialHealth;
    public int Health;

    public TextMeshProUGUI HealthDisplay;
    
    // Start is called before the first frame update
    void Start()
    {
        Health = InitialHealth;
        UpdateHealthDisplay();
    }

    public void DamagePlayer(int dmg)
    {
        Health -= dmg;
        UpdateHealthDisplay();
    }

    public void UpdateHealthDisplay()
    {
        HealthDisplay.text = Health.ToString();
    }

    public void ResetHealth()
    {
        Health = InitialHealth;
        UpdateHealthDisplay();
    }
}
