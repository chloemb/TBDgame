using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Collider2D bulletCol;
    private SpriteRenderer bulletRenderer;
    public Rigidbody2D playerOrigin;

    void Start()
    {
        bulletCol = GetComponent<Collider2D>();
        bulletRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnBecameInvisible()
    {
        Invoke("DestroyBullet", gameObject.GetComponent<Damager>().KnockbackLength);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (bulletRenderer && bulletCol)
        {
            bulletRenderer.enabled = bulletCol.enabled = false;
        }

        Invoke("DestroyBullet", gameObject.GetComponent<Damager>().KnockbackLength);
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}