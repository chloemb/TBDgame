using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public AudioSource PowerupAudioSource;
    public AudioClip GetPowerup;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Reactor>().PickUpPowerup(gameObject);
            if (!PowerupAudioSource.isPlaying)
                PowerupAudioSource.PlayOneShot(GetPowerup);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            Invoke("RemovePowerup", GetPowerup.length);  
        }
    }

    private void RemovePowerup()
    {
        Destroy(gameObject);
    }
}
