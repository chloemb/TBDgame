using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyHolder : MonoBehaviour
{
    public float Lifetime;
    
    void Awake()
    {
        Invoke("Remove", Lifetime);
    }

    private void Remove()
    {
        Destroy(gameObject);
    }
}
