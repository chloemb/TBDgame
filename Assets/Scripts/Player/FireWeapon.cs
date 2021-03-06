﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class FireWeapon : MonoBehaviour
{
    public GameObject[] Bullets;
    public int RemainingUses;

    public string CurrentOffhandWeapon;
    
    public float Speed, Cooldown, OffhandSpeed, OffhandCooldown;
    public Vector2 SummonPoint;
    private bool OnCooldown, OffhandOnCooldown;

    private static int DEFAULT_BULLET = 0;
    private static int BUBBLET = 1;
    private static int UNSTOPPABULLET = 2;
    private static int BOUNCLET = 3;

    [HideInInspector] public bool CurrentlyFiring;

    private Vector2 FireDirection;
    private SpriteRenderer _aimindic;
    
    // Audio
    public AudioSource WeaponAudioSource;
    public AudioClip FireDefault, FireBubble, FireDrill, FireGrenade;

    private void Start()
    {
        BulletInfo info = Bullets[DEFAULT_BULLET].GetComponent<BulletInfo>();
        Speed = info.Speed;
        Cooldown = info.Cooldown;
        
        _aimindic = gameObject.transform.Find("Canvas").Find("AimIndicator").GetComponent<SpriteRenderer>();
    }

    public void Fire(Vector2 FireDirection)
    {
        if (!OnCooldown)
        {
            this.FireDirection = FireDirection;
            
            SummonBullet(DEFAULT_BULLET, Speed);
            WeaponAudioSource.PlayOneShot(FireDefault);
            
            CurrentlyFiring = true;
            Invoke("NoLongerFiring", .05f);
            OnCooldown = OffhandOnCooldown = true;
            Invoke("RefreshShootCooldown", Cooldown);
            Invoke("RefreshOffhandCooldown", OffhandCooldown);
        }
    }

    public void FireOffhand(Vector2 FireDirection)
    {
        if (!OffhandOnCooldown && CurrentOffhandWeapon != null)
        {
            if (RemainingUses == 0)
            {
                SwitchWeapon(null);
            }
            else
            {

                this.FireDirection = FireDirection;

                switch (CurrentOffhandWeapon)
                {
                    case "Bubble Gun":
                        SummonBullet(BUBBLET, OffhandSpeed);
                        WeaponAudioSource.PlayOneShot(FireBubble);
                        RemainingUses--;
                        break;
                    case "Gunstoppable":
                        SummonBullet(UNSTOPPABULLET, OffhandSpeed);
                        WeaponAudioSource.PlayOneShot(FireDrill);
                        RemainingUses--;
                        break;
                    case "Bouncing Bomb":
                        SummonBullet(BOUNCLET, OffhandSpeed);
                        WeaponAudioSource.PlayOneShot(FireGrenade);
                        RemainingUses--;
                        break;
                }

                CurrentlyFiring = true;
                Invoke("NoLongerFiring", .05f);
                OffhandOnCooldown = OnCooldown = true;
                Invoke("RefreshOffhandCooldown", OffhandCooldown);
                Invoke("RefreshShootCooldown", Cooldown);
            }
        }
    }

    private void SummonBullet(int BulletIndex, float CertainSpeed)
    {
        Vector3 RelativeSumPoint = new Vector3(SummonPoint.x * GetComponent<Collider2D>().bounds.size.x,
            SummonPoint.y * GetComponent<Collider2D>().bounds.size.y, 0);
        Vector3 position;
        Quaternion rotation;
        bool FacingRight = GetComponent<PlayerController>().FacingRight;

        if (FireDirection.y < 0 && FireDirection.x == 0)
            position = FacingRight
                ? transform.position + RelativeSumPoint
                : transform.position + Vector3.Reflect(RelativeSumPoint, Vector3.right);
        else position = _aimindic.transform.position;

        rotation = FacingRight
            ? Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.right, FireDirection))
            : Quaternion.Euler(0f, 180f, Vector2.SignedAngle(FireDirection, Vector2.left));
        
        var bulletInstance = Instantiate(Bullets[BulletIndex], position, rotation);
        bulletInstance.GetComponent<Rigidbody2D>().velocity = CertainSpeed * FireDirection;
        bulletInstance.gameObject.GetComponent<BulletInfo>().playerOrigin = gameObject;
    }

    public void RefreshShootCooldown()
    {
        OnCooldown = false;
    }

    public void RefreshOffhandCooldown()
    {
        OffhandOnCooldown = false;
    }
    
    private void NoLongerFiring()
    {
        CurrentlyFiring = false;
    }

    public void SwitchWeapon(string newgun)
    {
        BulletInfo info = null;
        switch (newgun)
        {
            case "Bubble Gun":
                CurrentOffhandWeapon = "Bubble Gun";
                info = Bullets[BUBBLET].GetComponent<BulletInfo>();
                RemainingUses = 3;
                GetComponent<PlayerController>().CanShootInWall = false;
                break;
            case "Gunstoppable":
                CurrentOffhandWeapon = "Gunstoppable";
                info = Bullets[UNSTOPPABULLET].GetComponent<BulletInfo>();
                RemainingUses = 5;
                GetComponent<PlayerController>().CanShootInWall = true;
                break;
            case "Bouncing Bomb":
                CurrentOffhandWeapon = "Bouncing Bomb";
                info = Bullets[BOUNCLET].GetComponent<BulletInfo>();
                RemainingUses = 5;
                GetComponent<PlayerController>().CanShootInWall = false;
                break;
            case null:
                CurrentOffhandWeapon = null;
                GetComponent<PlayerController>().CanShootInWall = false;
                break;
        }

        if (info != null)
        {
            OffhandSpeed = info.Speed;
            OffhandCooldown = info.Cooldown;
        }
    }
}
