using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public String playerOrigin;
    public float Lifetime;
    
    void Awake()
    {
        Invoke("DestroyTrap", Lifetime);
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && playerOrigin != other.name)
            DestroyTrap();
    }
    
    public void DestroyTrap()
    {
        Destroy(gameObject);
    }
}
