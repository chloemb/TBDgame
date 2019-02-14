using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Collider2D bulletCol;
    private SpriteRenderer bulletRenderer;
    
    void Start()
    {
        bulletCol = GetComponent<Collider2D>();
        bulletRenderer = GetComponent<SpriteRenderer>();
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        bulletCol.enabled = false;
        bulletRenderer.enabled = false;
        
        Invoke("DestroyBullet", gameObject.GetComponent<Damager>().KnockbackLength);
    }
    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
