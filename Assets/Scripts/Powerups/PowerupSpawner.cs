using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PowerupSpawner : MonoBehaviour
{
    private EdgeCollider2D[] Floors;
    public GameObject Powerup;

    private void Start()
    {
        Floors = GetComponents<EdgeCollider2D>();
        Invoke("SpawnPowerup", 5);
    }

    private void SpawnPowerup()
    {
        Random rnd = new Random();
        
        // Pick random floor except center one
        EdgeCollider2D spawnfloor = Floors[rnd.Next(1, Floors.Length)];
        Instantiate(Powerup, (spawnfloor.points[0]+spawnfloor.points[1])/2 + new Vector2(0,.5f), Quaternion.identity);
        Invoke("SpawnPowerup", rnd.Next(20,30));
    }
}