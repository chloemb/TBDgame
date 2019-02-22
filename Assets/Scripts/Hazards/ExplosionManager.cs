using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    public GameObject ExplodablePieces;
    public float MaxDebrisSpeed;
    public float MaxContactSpeed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Projectiles")
            Explode();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Projectiles" || col.relativeVelocity.magnitude > MaxContactSpeed)
            Explode();
    }

    public void Explode()
    {
        Destroy(gameObject);
        GameObject pieces = Instantiate(ExplodablePieces, transform.position, Quaternion.identity);
        foreach (Transform p in pieces.transform)
        {
            p.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-MaxDebrisSpeed, MaxDebrisSpeed), Random.Range(-MaxDebrisSpeed, MaxDebrisSpeed));
        }
    }
}
