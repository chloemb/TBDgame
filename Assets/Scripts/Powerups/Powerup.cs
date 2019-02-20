using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        other.gameObject.GetComponent<HealthManager>().DamagePlayer(-1);
        Destroy(gameObject);
    }
}
