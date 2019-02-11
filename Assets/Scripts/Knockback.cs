using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerController controller;
    
    // customizable in inspecotr
    public float KnockbackStrength;
    public float KnockbackLength;

    private Vector2 LastKnocked;
    private bool KnockingBack;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
    }

    public void KnockPlayer()
    {
        // Calculate velocity and magnitude of knockback
        LastKnocked = -KnockbackStrength * controller.PrevVelocity.normalized;
        LastKnocked.y = 2f * LastKnocked.y;
        
        KnockingBack = true;
        
        // Apply knockback force
        _rb.velocity = LastKnocked;
        
        // Disable control while knocking back; give it back after KnockbackLength
        controller.DisableControl();
        Invoke("StopKnocking", KnockbackLength);
    }

    private void StopKnocking()
    {
        controller.GiveBackControl();
        KnockingBack = false;
    }
}
