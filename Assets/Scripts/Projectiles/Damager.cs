using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    // customizable in inspector
    public float KnockbackStrength, KnockbackLength, IFrames;
    public int Damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Reactor>().React(gameObject, Damage, KnockbackStrength, KnockbackLength, IFrames);
        }
    }
}