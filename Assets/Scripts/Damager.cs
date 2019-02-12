using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    private Rigidbody2D _playerrb;
    private PlayerController _playerController;
    
    // customizable in inspector
    public float KnockbackStrength;
    public float KnockbackLength;
    public int Damage;

    private Vector2 LastKnocked;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<HealthManager>().DamagePlayer(Damage);
            _playerrb = other.gameObject.GetComponent<Rigidbody2D>();
            _playerController = other.gameObject.GetComponent<PlayerController>();
            KnockPlayer();
        }
    }

    public void KnockPlayer()
    {
        // Calculate velocity and magnitude of knockback
        LastKnocked = -KnockbackStrength * _playerController.PrevVelocity.normalized;
        LastKnocked.y = 2f * LastKnocked.y;

        _playerController.KnockingBack = true;
        
        // Apply knockback force
        _playerrb.velocity = LastKnocked;
        
        // Disable control while knocking back; give it back after KnockbackLength
        _playerController.DisableControl();
        Invoke("StopKnocking", KnockbackLength);
    }

    private void StopKnocking()
    {
        _playerController.GiveBackControl();
        _playerController.KnockingBack = false;
    }
}
