using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    private Rigidbody2D _playerrb;
    private PlayerController _playerController;
    private HealthManager _playerHealthManager;

    // customizable in inspector
    public float KnockbackStrength, KnockbackLength, IFrames;
    public int Damage;

    private bool AppliedDamage;
    private Vector2 _lastKnocked;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !other.GetComponent<HealthManager>().CurrentlyInvincible)
        {
            _playerrb = other.gameObject.GetComponent<Rigidbody2D>();
            _playerController = other.gameObject.GetComponent<PlayerController>();
            _playerHealthManager = other.gameObject.GetComponent<HealthManager>();
            _playerHealthManager.LastHitIFrames = IFrames;
            _playerHealthManager.MakeInvincible(IFrames);
            _playerHealthManager.DamagePlayer(Damage);
            AppliedDamage = gameObject.CompareTag("Hazards");
            KnockPlayer();
        }
        
        AppliedDamage = false;
    }

    public void KnockPlayer()
    {
        // Calculate velocity and magnitude of knockback
        if (AppliedDamage)
        {
            _lastKnocked = -KnockbackStrength * _playerController.PrevVelocity.normalized;
            _lastKnocked.y *= 2f;
        }
        else _lastKnocked.x = gameObject.GetComponent<Rigidbody2D>().velocity.x;

        _playerController.KnockingBack = true;

        // Apply knockback force
        _playerrb.velocity = _lastKnocked;

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