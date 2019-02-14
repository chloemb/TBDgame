using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeapon : MonoBehaviour
{
    public Rigidbody2D Bullet;
    public float Speed = 1f;
    
    public void FireDefaultWeapon(bool facingRight, GameObject player)
    {
        var playerWidth = new Vector3(player.GetComponent<Collider2D>().bounds.size.x, 0, 0);
        if(facingRight)
        {
            Rigidbody2D bulletInstance = Instantiate(Bullet, transform.position + playerWidth, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
            bulletInstance.velocity = new Vector2(Speed, 0);
        }
        else
        {
            Rigidbody2D bulletInstance = Instantiate(Bullet, transform.position - playerWidth, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
            bulletInstance.velocity = new Vector2(-Speed, 0);
        }
    }
}
