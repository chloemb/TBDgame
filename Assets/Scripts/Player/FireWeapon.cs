using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class FireWeapon : MonoBehaviour
{
    public GameObject[] Bullets;
    public float Speed = 1f;
    public Vector2 SummonPoint;
    public float Cooldown;
    private bool OnCooldown;

    public float RemainingUses;

    public string CurrentWeapon;

    private static int DEFAULT_BULLET = 0;
    private static int BUBBLET = 1;

    [HideInInspector] public bool CurrentlyFiring;

    private Vector2 FireDirection;

    private void Start()
    {
        CurrentWeapon = "Default";

        BulletInfo info = Bullets[DEFAULT_BULLET].GetComponent<BulletInfo>();
        Speed = info.Speed;
        Cooldown = info.Cooldown;
    }

    public void Fire(Vector2 FireDirection)
    {
        if (!OnCooldown)
        {
            this.FireDirection = FireDirection;
            
            switch (CurrentWeapon)
            {
                case "Default":
                    SummonBullet(DEFAULT_BULLET);
                    break;
                
                case "Bubble Gun":
                    SummonBullet(BUBBLET);
                    SwitchWeapon("Default");
                    break;
            }
            
            CurrentlyFiring = true;
            OnCooldown = true;
            Invoke("RefreshShootCooldown", Cooldown);
        }
    }

    private void SummonBullet(int BulletIndex)
    {
        Invoke("NoLongerFiring", .05f);
        Vector3 RelativeSumPoint = new Vector3(SummonPoint.x * GetComponent<Collider2D>().bounds.size.x,
            SummonPoint.y * GetComponent<Collider2D>().bounds.size.y, 0);
        Vector3 position;
        Quaternion rotation;
        
        if (FireDirection.x >= 0 && GetComponent<PlayerController>().FacingRight)
        {
            position = transform.position + RelativeSumPoint;
            rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.right, FireDirection));
        }
        else
        {
            position = transform.position + Vector3.Reflect(RelativeSumPoint, Vector3.right);
            rotation = Quaternion.Euler(0f, 180f, Vector2.SignedAngle(FireDirection, Vector2.left));
        }
        var bulletInstance = Instantiate(Bullets[BulletIndex], position, rotation);
        bulletInstance.GetComponent<Rigidbody2D>().velocity = Speed * FireDirection;
        bulletInstance.gameObject.GetComponent<BulletInfo>().playerOrigin = gameObject;
    }

    public void RefreshShootCooldown()
    {
        OnCooldown = false;
    }
    
    
    private void NoLongerFiring()
    {
        CurrentlyFiring = false;
    }

    public void SwitchWeapon(string newgun)
    {
        BulletInfo info;
        switch (newgun)
        {
            case "Default":
                CurrentWeapon = "Default";
                info = Bullets[DEFAULT_BULLET].GetComponent<BulletInfo>();
                break;
            case "Bubble Gun":
                CurrentWeapon = "Bubble Gun";
                info = Bullets[BUBBLET].GetComponent<BulletInfo>();
                break;
            default:
                CurrentWeapon = "Default";
                info = Bullets[DEFAULT_BULLET].GetComponent<BulletInfo>();
                break;
        }
        
        Speed = info.Speed;
        Cooldown = info.Cooldown;
    }
}
