using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallManager : MonoBehaviour
{
    public float FallSpeed;
    void Awake()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Damager>().enabled = false;
    }

    public void Fall()
    {
        GetComponent<Rigidbody2D>().gravityScale = FallSpeed;
        GetComponent<Damager>().enabled = true;
    }
}
