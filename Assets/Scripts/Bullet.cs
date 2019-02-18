using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Collider2D bulletCol;
    private SpriteRenderer bulletRenderer;
    public Rigidbody2D playerOrigin;
    public float BulletLife;

    void Awake()
    {
        bulletCol = GetComponent<Collider2D>();
        bulletRenderer = GetComponent<SpriteRenderer>();
        Invoke("PrepareDestroyBullet", BulletLife);
    }

    private void OnBecameInvisible()
    {
        Invoke("DestroyBullet", gameObject.GetComponent<Damager>().KnockbackLength);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Turn off bullet renderer and collider; destroy it after KnockbackLength
        if (!col.gameObject.CompareTag("Hazards")) PrepareDestroyBullet();
    }

    private void PrepareDestroyBullet()
    {
        bulletRenderer.enabled = bulletCol.enabled = false;
        Invoke("DestroyBullet", gameObject.GetComponent<Damager>().KnockbackLength);
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}