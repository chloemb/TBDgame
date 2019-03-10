using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounclet : MonoBehaviour
{
    public GameObject ExplosionPrefab;
    public GameObject origin;
    public float Countdown;

    void Awake()
    {
        origin = gameObject.GetComponent<BulletInfo>().playerOrigin;
        Invoke("Detonate", Countdown);
    }
    
    private void Detonate()
    {
        Destroy(this.gameObject);
        var explosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity, transform.parent);
        var pos = explosion.transform.position;
        var rad = explosion.GetComponent<CircleCollider2D>().radius;
        Collider2D[] collidersInRadius = Physics2D.OverlapCircleAll(pos, rad);
        foreach (Collider2D col in collidersInRadius)
        {
            if (col.gameObject.name.Contains("Falling Spike"))
                col.gameObject.GetComponent<FallManager>().Fall();
            if (col.gameObject.name.Contains("Exploding Box"))
                col.gameObject.GetComponent<ExplosionManager>().Explode(origin);
        }
    }
}
