using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Experimental.UIElements;
using Random = System.Random;

public class HazardSpawner : MonoBehaviour
{
    public List<Transform> BoxSpawnPoints;
    public List<Transform> SpikeSpawnPoints;
    public GameObject[] Players;

    public GameObject ExplodingBox, FallingSpike;

    public int SpikeSpawnRate, BoxSpawnRate;

    private void Start()
    {
        foreach (var child in GameObject.Find("Exploding Boxes").GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name.Contains("Spawn"))
            {
                BoxSpawnPoints.Add(child);
            }
        }
        foreach (var child in GameObject.Find("Falling Spikes").GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name.Contains("Spawn"))
            {
                SpikeSpawnPoints.Add(child);
            }
        }
    }

    private void FixedUpdate()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        bool playerhere = false;
        
        Random rnd = new Random();

        foreach (Transform boxspawn in BoxSpawnPoints)
        {
            if (boxspawn.childCount == 0)
            {
                int spawnhere = rnd.Next(0, BoxSpawnRate);
                if (spawnhere == 0)
                {
                    Vector3 boxsize = new Vector3(boxspawn.localScale.x, 0);
                    Debug.Log(boxsize);
                    
                    foreach (GameObject player in Players)
                    {
                        if (player.GetComponent<Collider2D>().bounds.Contains(boxspawn.position + 2*boxsize) ||
                            player.GetComponent<Collider2D>().bounds.Contains(boxspawn.position ) ||
                            player.GetComponent<Collider2D>().bounds.Contains(boxspawn.position - 2*boxsize))
                        {
                            playerhere = true;
                        }
                    }

                    if (!playerhere)
                    {
                        Instantiate(ExplodingBox, boxspawn);
                    }
                }
            }
        }

        foreach (Transform spikespawn in SpikeSpawnPoints)
        {
            if (spikespawn.childCount == 0)
            {
                int spawnhere = rnd.Next(0, SpikeSpawnRate);
                if (spawnhere == 0)
                {
                    foreach (GameObject player in Players)
                    {
                        if (player.GetComponent<Collider2D>().bounds.Contains(spikespawn.position))
                        {
                            playerhere = true;
                        }
                    }

                    if (!playerhere)
                    {
                        Instantiate(FallingSpike, spikespawn);
                    }
                }
            }
        }
    }
}
