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
            AudioSource.PlayClipAtPoint(GetPowerup, 0.9f*Camera.main.transform.position + 0.1f*transform.position, 2f);
            Destroy(gameObject);
        }
    }
}
