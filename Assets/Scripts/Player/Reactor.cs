using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactor : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerController _pc;
    private HealthManager _hm;

    private float KnockbackStrength, KnockbackLength;

    private bool HitHazard;
    private Vector2 _lastKnocked;

    private GameObject LastImpact;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _pc = GetComponent<PlayerController>();
        _hm = GetComponent<HealthManager>();
    }

    public void React(GameObject effector, int Damage, float KnockbackStrength, float KnockbackLength, float IFrames)
    {
        if (!_hm.CurrentlyInvincible && !_pc.CurrentlyDashing)
        {
            LastImpact = effector;

            _hm.LastHitIFrames = IFrames;
            _hm.MakeInvincible(IFrames);
            _hm.DamagePlayer(Damage);

            this.KnockbackStrength = KnockbackStrength;
            this.KnockbackLength = KnockbackLength;
            HitHazard = effector.CompareTag("Hazards");
            KnockPlayer();
        }
    }

    public void KnockPlayer()
    {
        // Disable control while knocking back; give it back after KnockbackLength
        _pc.DisableControl();
        Invoke("StopKnocking", KnockbackLength);
        
        // Knockback from hazard
        if (HitHazard)
        {
            _lastKnocked = -KnockbackStrength * _pc.PrevVelocity.normalized;
            _lastKnocked.y *= 2f;
        }
        // Knockback from weapon
        else _lastKnocked = KnockbackStrength * LastImpact.GetComponent<Rigidbody2D>().velocity;

        _pc.KnockingBack = true;

        // Apply knockback force
        _rb.velocity = _lastKnocked;
    }

    private void StopKnocking()
    {
        _pc.GiveBackControl();
        _pc.KnockingBack = false;
    }
}