using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public GameObject playerOrigin;
    public float Lifetime;
    
    void Awake()
    {
        Invoke("DestroyTrap", Lifetime);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && playerOrigin.name != other.name)
            DestroyTrap();
    }
    
    private void DestroyTrap()
    {
        if (gameObject != null)
            Destroy(gameObject);
    }
}
