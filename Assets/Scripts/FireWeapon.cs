using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeapon : MonoBehaviour
{
    public Rigidbody2D Bullet;
    public float Speed = 1f;
    public Vector2 BulletPos;
    public float Cooldown;
    private bool OnCooldown;

    private Vector2 FireDirection;
    
    public void FireDefaultWeapon(bool facingright, Vector2 FireDirection, GameObject player)
    {
        if (!OnCooldown)
        {
            this.FireDirection = FireDirection;
            OnCooldown = true;
            Invoke("RefreshShootCooldown", Cooldown);
            var playerWidth = new Vector3(BulletPos.x * player.GetComponent<CapsuleCollider2D>().bounds.size.x,
                BulletPos.y);

            if (facingright)
            {
                SummonBullet(transform.position + playerWidth, 0);

            }
            else
            {
                SummonBullet(new Vector3(transform.position.x - playerWidth.x, transform.position.y + playerWidth.y), 180);
            }
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
}
