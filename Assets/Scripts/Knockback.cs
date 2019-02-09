using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    private Rigidbody2D _rb;

    public float KnockbackStrength;

    public Vector2 LastKnocked;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KnockPlayer()
    {
        PlayerController controller = _rb.GetComponent<PlayerController>();
        LastKnocked = -KnockbackStrength * controller._prevVelocity.normalized;
        if (controller.IsGrounded)
        {
            LastKnocked.y = KnockbackStrength;
            LastKnocked.x = KnockbackStrength * LastKnocked.x;
        }

        _rb.velocity = LastKnocked;

    }
}
