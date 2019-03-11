using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    // customizable in inspector
    public float EffectStrength, EffectLength, IFrames;
    public int Damage;
    public bool NoDamageToSelf;

    public bool IgnorePlayerOrigin;

    private void Awake()
    {
        if (gameObject.name.Contains("Box Piece") || gameObject.name.Contains("Trap"))
            NoDamageToSelf = true;

        IgnorePlayerOrigin = true;
        Invoke("AffectPlayer", .1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !NoDamageToSelf)
        {
            if (!(gameObject.CompareTag("Projectiles") &&
                  other.gameObject == GetComponent<BulletInfo>().playerOrigin && IgnorePlayerOrigin))
            {
                other.gameObject.GetComponent<Reactor>().React(gameObject);
            }
        }

        if (other.gameObject.name.Contains("Falling Spike"))
            other.gameObject.GetComponent<FallManager>().Fall();

        if (other.gameObject.CompareTag("Player") && gameObject.name.Contains("Box Piece"))
        {
            if (gameObject.GetComponent<BoxPiece>().playerOrigin == null ||
                gameObject.GetComponent<BoxPiece>().playerOrigin != null &&
                gameObject.GetComponent<BoxPiece>().playerOrigin.name != other.name)
                other.gameObject.GetComponent<Reactor>().React(gameObject);
        }
        
        if (other.gameObject.CompareTag("Player") && gameObject.name.Contains("Trap") &&
            gameObject.GetComponent<Trap>().playerOrigin != null &&
            gameObject.GetComponent<Trap>().playerOrigin.name != other.name)
                other.gameObject.GetComponent<Reactor>().React(gameObject);
    }

    private void AffectPlayer()
    {
        IgnorePlayerOrigin = false;
    }
}