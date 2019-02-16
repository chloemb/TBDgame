using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class FireWeapon : MonoBehaviour
{
    public Rigidbody2D Bullet;
    public float Speed = 1f;
    public Vector2 SummonPoint;
    public float Cooldown;
    private bool OnCooldown;

    [HideInInspector] public bool CurrentlyFiring;

    private Vector2 FireDirection;

    public void FireDefaultWeapon(bool facingright, Vector2 FireDirection, GameObject player)
    {
        if (!OnCooldown)
        {
            CurrentlyFiring = true;
            Invoke("NoLongerFiring", .05f);
            
            this.FireDirection = FireDirection;
            Vector3 RelativeSumPoint = new Vector3 (SummonPoint.x * player.GetComponent<CapsuleCollider2D>().bounds.size.x,
                SummonPoint.y * player.GetComponent<CapsuleCollider2D>().bounds.size.y, 0);

            if (facingright) SummonBullet(transform.position + RelativeSumPoint, 0);
            else SummonBullet(transform.position + Vector3.Reflect(RelativeSumPoint, Vector3.right), 180);
            
            OnCooldown = true;
            Invoke("RefreshShootCooldown", Cooldown);
        }
    }

    private void SummonBullet(Vector3 position, float rotation)
    {
        Rigidbody2D bulletInstance = Instantiate(Bullet, position, Quaternion.Euler(new Vector3(0, rotation, 0)));
        bulletInstance.velocity = Speed * FireDirection;
        bulletInstance.gameObject.GetComponent<Bullet>().playerOrigin = GetComponent<Rigidbody2D>();
    }
    
    public void RefreshShootCooldown()
    {
        OnCooldown = false;
    }
    
    
    private void NoLongerFiring()
    {
        CurrentlyFiring = false;
    }
}
