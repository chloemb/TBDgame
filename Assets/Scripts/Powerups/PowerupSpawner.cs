using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PowerupSpawner : MonoBehaviour
{
    public List<Transform> PowerupSpawnPoints;
    public GameObject[] Powerups;

    public float FirstSpawn;
    public int SpawnIntervalMinimum;
    public int SpawnIntervalMaximum;

    private void Start()
    {
        foreach (var child in GameObject.Find("Powerups").GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name.Contains("Spawn"))
            {
                PowerupSpawnPoints.Add(child);
            }
        }
        Invoke("SpawnPowerup", FirstSpawn);
    }

    private void SpawnPowerup()
    {
        Random rnd = new Random();
        
        // Pick random spawnpoint
        Transform spawnpoint = PowerupSpawnPoints[rnd.Next(1, PowerupSpawnPoints.Count)];
        
        // Pick random type of powerup
        GameObject powerup = Powerups[rnd.Next(0, Powerups.Length)];
        
        // Spawn that powerup at chosen point
        var newpowerup = Instantiate(powerup, spawnpoint);
        newpowerup.transform.Translate(0f, .5f, 0f);
        
        // Set up next powerup spawn
        Invoke("SpawnPowerup", rnd.Next(SpawnIntervalMinimum, SpawnIntervalMaximum));
    }
}