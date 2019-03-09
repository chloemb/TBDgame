using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounclet : MonoBehaviour
{
    public GameObject ExplosionPrefab;
    public GameObject playerOrigin;
    public float Countdown;

    void Awake()
    {
        Invoke("Detonate", Countdown);
    }
    
    private void Detonate()
    {
        Destroy(this.gameObject);
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity, transform.parent);
    }
}
