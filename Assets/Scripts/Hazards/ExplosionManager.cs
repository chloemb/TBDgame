using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    public GameObject ExplodablePieces;
    public GameObject ExplosionPrefab;
    public float MaxDebrisSpeed;
    public float MaxContactSpeed;
    public AudioClip BoxExplosion;

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
        }
    }

    public void Explode(GameObject origin)
    {
        AudioSource.PlayClipAtPoint(BoxExplosion, 0.9f*Camera.main.transform.position + 0.1f*transform.position, 10f);
        Destroy(gameObject);
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity, transform.parent);
        GameObject pieces = Instantiate(ExplodablePieces, transform.position, Quaternion.identity);
        foreach (Transform p in pieces.transform)
        {
            p.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-MaxDebrisSpeed, MaxDebrisSpeed), Random.Range(-MaxDebrisSpeed, MaxDebrisSpeed));
            if (origin != null)
            {
                p.GetComponent<BoxPiece>().playerOrigin = origin;
                var boxParticleSystem = p.gameObject.GetComponent<ParticleSystem>().main;
                if (origin.name == "Player 1")
                    boxParticleSystem.startColor = new Color(93f/255f, 96f/255f, 244f/255f);
                else if (origin.name == "Player 2")
                    boxParticleSystem.startColor = new Color(255f/255f, 144f/255f, 0);
            }
        }
    }
}
