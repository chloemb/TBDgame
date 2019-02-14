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
    public bool AppliedDamage;

    private Vector2 _lastKnocked;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (gameObject.tag == "Hazards")
            {
                other.gameObject.GetComponent<HealthManager>().DamagePlayer(Damage);
                AppliedDamage = true;
            }
            _playerrb = other.gameObject.GetComponent<Rigidbody2D>();
            _playerController = other.gameObject.GetComponent<PlayerController>();
            KnockPlayer();
        }

        AppliedDamage = false;
    }

    public void KnockPlayer()
    {
        Debug.Log("knock player called");
        // Calculate velocity and magnitude of knockback
        if (AppliedDamage)
        {
            _lastKnocked = -KnockbackStrength * _playerController.PrevVelocity.normalized;
            _lastKnocked.y = 2f * _lastKnocked.y;
        }
        else
        {
            Debug.Log(gameObject.GetComponent<Rigidbody2D>().velocity);
            _lastKnocked.x = gameObject.GetComponent<Rigidbody2D>().velocity.x;
        }

        _playerController.KnockingBack = true;
        
        // Apply knockback force
        _playerrb.velocity = _lastKnocked;

        // Disable control while knocking back; give it back after KnockbackLength
        _playerController.DisableControl();

        Invoke("StopKnocking", KnockbackLength);
        Debug.Log(KnockbackLength);
    }

    private void StopKnocking()
    {
        _playerController.GiveBackControl();
        _playerController.KnockingBack = false;
    }
}
