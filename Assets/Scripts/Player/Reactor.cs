using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactor : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerController _pc;
    private HealthManager _hm;

    private float EffectStrength, EffectLength;

    private bool HitHazard;
    private Vector2 _lastKnocked;

    private GameObject LastImpact;

    public bool KnockingBack, Floating;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _pc = GetComponent<PlayerController>();
        _hm = GetComponent<HealthManager>();
    }

    public void React(GameObject effector)
    {
        Damager effectorinfo = effector.GetComponent<Damager>();
        
        if (!_hm.CurrentlyInvincible)
        {
            LastImpact = effector;
            EffectStrength = effectorinfo.EffectStrength;
            EffectLength = effectorinfo.EffectLength;

            if (effector.CompareTag("Projectiles"))
            {
                HitHazard = false;
                switch (effector.name)
                {
                    case "Default Bullet(Clone)":
                        HitHazard = false;
                        KnockPlayer();
                        break;
                    case "Bubblet(Clone)":
                        FloatPlayer();
                        break;
                    case "Unstoppabullet(Clone)":
                        HitHazard = false;
                        KnockPlayer();
                        break;
                }
            }
            else if (!_pc.CurrentlyDashing)
            {
                _hm.LastHitIFrames = effectorinfo.IFrames;
                _hm.MakeInvincible(effectorinfo.IFrames);
                _hm.DamagePlayer(effectorinfo.Damage);

                HitHazard = true;
                KnockPlayer();
            }
        }
    }

    public void KnockPlayer()
    {
        // Disable control while knocking back; give it back after KnockbackLength
        _pc.DisableControl();
        KnockingBack = true;
        Invoke("Release", EffectLength);

        // Knockback from hazard
        if (HitHazard)
        {
            _lastKnocked = -EffectStrength * _pc.PrevVelocity.normalized;
            _lastKnocked.y *= 2f;
        }
        // Knockback from weapon
        else _lastKnocked = EffectStrength * LastImpact.GetComponent<Rigidbody2D>().velocity;

        // Apply knockback force
        _rb.velocity = _lastKnocked;
    }
    
    private void FloatPlayer()
    {
        _pc.DisableControl();
        Floating = true;
        Invoke("Release", EffectLength);

        _rb.gravityScale = 0f;
        _rb.velocity = new Vector2(0f, .2f * EffectStrength);
    }

    private void Release()
    {
        _pc.GiveBackControl();
        KnockingBack = false;
        Floating = false;
        _rb.gravityScale = _pc.GravityScale;
    }

    public void PickUpPowerup(GameObject powerup)
    {
        switch (powerup.name)
        {
            case "Health Pack(Clone)":
                GetComponent<HealthManager>().DamagePlayer(-1);
                break;

            case "Bubble Gun Powerup(Clone)":
                GetComponent<FireWeapon>().SwitchWeapon("Bubble Gun");
                break;
            
            case "Gunstoppable Powerup(Clone)":
                GetComponent<FireWeapon>().SwitchWeapon("Gunstoppable");
                break;
            case "Bouncing Bomb Powerup(Clone)":
                GetComponent<FireWeapon>().SwitchWeapon("Bouncing Bomb");
                break;
        }
    }
}