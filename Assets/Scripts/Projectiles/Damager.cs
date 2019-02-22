using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    // customizable in inspector
    public float EffectStrength, EffectLength, IFrames;
    public int Damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("calling react");
            other.gameObject.GetComponent<Reactor>().React(gameObject);
        }

        if (other.gameObject.name.Contains("Falling Spike"))
        {
            Debug.Log("hit falling spike");
            other.gameObject.GetComponent<FallManager>().Fall();
        }
    }
}