using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    private Rigidbody2D _playerrb;
    private PlayerController _playerController;
    
    // customizable in inspector
    public float KnockbackStrength, KnockbackLength, IFrames;
    public int Damage;
    
    private  bool AppliedDamage;
    private Vector2 _lastKnocked;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _playerrb = other.gameObject.GetComponent<Rigidbody2D>();
            _playerController = other.gameObject.GetComponent<PlayerController>();

            if (!_playerrb.GetComponent<HealthManager>().CurrentlyInvincible)
            {
                _playerrb.GetComponent<HealthManager>().MakeInvincible(IFrames);
                
                if (gameObject.tag == "Hazards")
                {
                    other.gameObject.GetComponent<HealthManager>().DamagePlayer(Damage);
                    AppliedDamage = true;
                }

                _playerrb = other.gameObject.GetComponent<Rigidbody2D>();
                _playerController = other.gameObject.GetComponent<PlayerController>();
                KnockPlayer();
            }
        }

        AppliedDamage = false;
    }

    public void KnockPlayer()
    {
        // Calculate velocity and magnitude of knockback
        if (AppliedDamage)
        {
            _lastKnocked = -KnockbackStrength * _playerController.PrevVelocity.normalized;
            _lastKnocked.y = 2f * _lastKnocked.y;
        }
        else
        {
            _lastKnocked.x = gameObject.GetComponent<Rigidbody2D>().velocity.x;
        }

        _playerController.KnockingBack = true;
        
        // Apply knockback force
        _playerrb.velocity = _lastKnocked;

        // Disable control while knocking back; give it back after KnockbackLength
        _playerController.DisableControl();
        
        // Start iframes
        IEnumerator toif = _playerrb.gameObject.GetComponent<AnimationController>().IFrameAnim(IFrames);
        StartCoroutine(toif);

        Invoke("StopKnocking", KnockbackLength);
    }

    private void StopKnocking()
    {
        _playerController.GiveBackControl();
        _playerController.KnockingBack = false;
    }
}
