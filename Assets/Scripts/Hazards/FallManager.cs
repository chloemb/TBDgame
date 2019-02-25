using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallManager : MonoBehaviour
{
    public float FallSpeed;

    private Rigidbody2D _rb;


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
    }

    public void Fall()
    {
        _rb.velocity = new Vector2(0, 0);
        _rb.gravityScale = FallSpeed;
    }
    
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
