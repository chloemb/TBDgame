using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PowerupSpawner : MonoBehaviour
{
    public List<Transform> PowerupSpawnPoints;
    public GameObject[] Powerups;

    public int PowerupSpawnRate;

    private void Start()
    {
        foreach (var child in GameObject.Find("Powerups").GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name.Contains("Spawn"))
            {
                PowerupSpawnPoints.Add(child);
            }
        }
    }
    
    private void FixedUpdate()
    {
        Random rnd = new Random();

        foreach (Transform puspawn in PowerupSpawnPoints)
        {
            if (puspawn.childCount == 0)
            {
                int spawnhere = rnd.Next(0, PowerupSpawnRate);
                if (spawnhere == 0)
                {
                    int whichpu = rnd.Next(0, Powerups.Length);
                    Instantiate(Powerups[whichpu], puspawn);
                }
            }
        }
    }
}