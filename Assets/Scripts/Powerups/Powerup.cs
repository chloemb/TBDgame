using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public AudioClip GetPowerup;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Reactor>().PickUpPowerup(gameObject);
            AudioSource.PlayClipAtPoint(GetPowerup, transform.position, 1.5f);
            Destroy(gameObject);
        }
    }
}
