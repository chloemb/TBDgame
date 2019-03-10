﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    public GameObject ExplodablePieces;
    public GameObject ExplosionPrefab;
    public float MaxDebrisSpeed;
    public float MaxContactSpeed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectiles"))
            Explode(other.GetComponent<BulletInfo>().playerOrigin);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player") && col.relativeVelocity.magnitude > MaxContactSpeed)
        {
            GameObject playerOrigin = col.collider.CompareTag("Projectiles") ? col.collider.GetComponent<BulletInfo>().playerOrigin : null;
            Explode(playerOrigin);
            col.gameObject.GetComponent<HealthManager>().DamagePlayer(1);
        }
    }

    public void Explode(GameObject origin)
    {
        Destroy(gameObject);
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity, transform.parent);
        GameObject pieces = Instantiate(ExplodablePieces, transform.position, Quaternion.identity);
        foreach (Transform p in pieces.transform)
        {
            p.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-MaxDebrisSpeed, MaxDebrisSpeed), Random.Range(-MaxDebrisSpeed, MaxDebrisSpeed));
            if (origin != null)
                p.GetComponent<BoxPiece>().playerOrigin = origin;
        }
    }
}
