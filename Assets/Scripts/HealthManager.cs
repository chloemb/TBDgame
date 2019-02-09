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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hazards")
        {
            Health -= 1;
        }
        
        UpdateHealthDisplay();
        Knockback kb = gameObject.GetComponent<Knockback>();
        kb.KnockPlayer();
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
