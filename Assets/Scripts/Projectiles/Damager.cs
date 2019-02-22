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
            other.gameObject.GetComponent<Reactor>().React(gameObject);
        }

        if (other.gameObject.name.Contains("Falling Spike"))
        {
            other.gameObject.GetComponent<FallManager>().Fall();
        }
    }
}