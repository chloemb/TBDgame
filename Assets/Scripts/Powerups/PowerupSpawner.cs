using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PowerupSpawner : MonoBehaviour
{
    private EdgeCollider2D[] Floors;
    public GameObject[] Powerups;

    public float FirstSpawn;
    public int SpawnIntervalMinimum;
    public int SpawnIntervalMaximum;

    private void Start()
    {
        Floors = GetComponents<EdgeCollider2D>();
        Invoke("SpawnPowerup", FirstSpawn);
    }

    private void SpawnPowerup()
    {
        Random rnd = new Random();
        
        // Pick random floor except center one
        EdgeCollider2D spawnfloor = Floors[rnd.Next(1, Floors.Length)];
        
        // Pick random type of powerup
        GameObject powerup = Powerups[rnd.Next(0, Powerups.Length)];
        
        // Spawn that powerup in middle of chosen floor
        Instantiate(powerup, (spawnfloor.points[0]+spawnfloor.points[1])/2 + new Vector2(0,.5f), Quaternion.identity);
        
        // Set up next powerup spawn
        Invoke("SpawnPowerup", rnd.Next(SpawnIntervalMinimum, SpawnIntervalMaximum));
    }
}