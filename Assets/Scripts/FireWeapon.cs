using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeapon : MonoBehaviour
{
    public Rigidbody2D Bullet;
    public float Speed = 1f;
    public Vector2 BulletPos;
    
    public void FireDefaultWeapon(bool facingright, Vector2 FireDirection, GameObject player)
    {
        var playerWidth = new Vector3(BulletPos.x * player.GetComponent<CapsuleCollider2D>().bounds.size.x, BulletPos.y);
        
        if(facingright)
        {
            Rigidbody2D bulletInstance = Instantiate(Bullet, transform.position + playerWidth, Quaternion.Euler(new Vector3(0,0,0)));
            bulletInstance.velocity = Speed * FireDirection;
        }
        else
        {
            Rigidbody2D bulletInstance = Instantiate(Bullet, new Vector3(transform.position.x - playerWidth.x, transform.position.y + playerWidth.y), Quaternion.Euler(new Vector3(0,0,180f)));
            bulletInstance.velocity = Speed * FireDirection;
        }
    }
}
