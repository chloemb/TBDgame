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
        int spawnpowerup = rnd.Next(0, PowerupSpawnRate * PowerupSpawnPoints.Count);

        if (spawnpowerup == 0)
        {
            int spawnhere = rnd.Next(0, PowerupSpawnPoints.Count);
            if (PowerupSpawnPoints[spawnhere].childCount == 0)
            {
                var newpowerup = Instantiate(Powerups[rnd.Next(0, Powerups.Length)], PowerupSpawnPoints[spawnhere]);
                newpowerup.transform.Translate(0f, .25f, 0f);
            }
        }
    }
}