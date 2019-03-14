using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallManager : MonoBehaviour
{
    public float FallSpeed;
    public AudioClip SpikeFall;

    private Rigidbody2D _rb;


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
    }

    public void Fall()
    {
        AudioSource.PlayClipAtPoint(SpikeFall, 0.9f*Camera.main.transform.position + 0.1f*transform.position, 15f);
        _rb.velocity = new Vector2(0, 0);
        _rb.gravityScale = FallSpeed;
    }
    
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
