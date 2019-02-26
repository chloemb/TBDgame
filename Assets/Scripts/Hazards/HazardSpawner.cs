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
        Random rnd = new Random();
        int spawnbox = rnd.Next(0, BoxSpawnRate * BoxSpawnPoints.Count);
        int spawnspike = rnd.Next(0, SpikeSpawnRate * SpikeSpawnPoints.Count);

        if (spawnbox == 0)
        {
            int spawnhere = rnd.Next(0, BoxSpawnPoints.Count);
            if (BoxSpawnPoints[spawnhere].childCount == 0)
            {
                Instantiate(ExplodingBox, BoxSpawnPoints[spawnhere]);
            }
        }
        
        if (spawnspike == 0)
        {
            int spawnhere = rnd.Next(0, SpikeSpawnPoints.Count);
            if (SpikeSpawnPoints[spawnhere].childCount == 0)
            {
                Instantiate(FallingSpike, SpikeSpawnPoints[spawnhere]);
            }
        }
    }
}
