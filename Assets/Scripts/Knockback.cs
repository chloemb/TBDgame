using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerController controller;

    public float KnockbackStrength;

    private Vector2 LastKnocked;
    private bool KnockingBack;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Give horizontal input back if done knocking back
        if (_rb.velocity.x == 0 && KnockingBack)
        {
            controller.IgnoreLeft = false;
            controller.IgnoreRight = false;
            KnockingBack = false;
        }
    }

    public void KnockPlayer()
    {
        // Calculate velocity and magnitude of knockback
        LastKnocked = -KnockbackStrength * controller.PrevVelocity.normalized;
        LastKnocked.y = 2f * LastKnocked.y;
        
        // Disallow horizontal movement while knocking back
        controller.IgnoreLeft = true;
        controller.IgnoreRight = true;
        KnockingBack = true;
        
        // Apply knockback force
        _rb.velocity = LastKnocked;

    }
}
