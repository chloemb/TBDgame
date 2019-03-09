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
    public GameObject[] Auras;

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
        Auras = GameObject.FindGameObjectsWithTag("Auras");
        bool playerhere = false;
        
        Random rnd = new Random();

        foreach (Transform boxspawn in BoxSpawnPoints)
        {
            if (boxspawn.childCount == 0)
            {
                int spawnhere = rnd.Next(0, BoxSpawnRate);
                if (spawnhere == 0)
                {
                    
                    foreach (GameObject area in Auras)
                    {
                        if (area.GetComponent<Collider2D>().bounds.Contains(boxspawn.position))
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
                    foreach (GameObject area in Auras)
                    {
                        if (area.GetComponent<Collider2D>().bounds.Contains(spikespawn.position))
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
