using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallManager : MonoBehaviour
{
    public float FallSpeed;
    void Awake()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    public void Fall()
    {
        GetComponent<Rigidbody2D>().gravityScale = FallSpeed;
    }
    
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
