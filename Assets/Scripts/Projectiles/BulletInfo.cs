using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInfo : MonoBehaviour
{
    private Collider2D bulletCol;
    private SpriteRenderer bulletRenderer;
    public GameObject playerOrigin;
    public float BulletLife;
    public float Cooldown, Uses, Speed;

    void Awake()
    {
        Invoke("DestroyBullet", BulletLife);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (gameObject.name.Contains("Unstoppabullet"))
        {
            if (col.gameObject.CompareTag("Player"))
                DestroyBullet();
        }
        else if (!col.gameObject.CompareTag("Powerups") && !col.gameObject.CompareTag("Hazards") &&
                 !col.gameObject.CompareTag("Projectiles"))
        {
            if (col.gameObject.CompareTag("Interactable Hazards") && !gameObject.name.Contains("Bounclet"))
                DestroyBullet();
            else if (!gameObject.name.Contains("Bounclet"))
                DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}