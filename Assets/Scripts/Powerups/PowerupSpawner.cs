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
    public float PowerupGraceLength;

    public List<bool> PowerupWasHere, PowerupGrace;

    private void Start()
    {
        foreach (var child in GameObject.Find("Powerups").GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name.Contains("Spawn"))
            {
                PowerupSpawnPoints.Add(child);
                PowerupWasHere.Add(false);
                PowerupGrace.Add(false);
            }
        }
    }
    
    private void FixedUpdate()
    {
        Random rnd = new Random();

        foreach (Transform puspawn in PowerupSpawnPoints)
        {
            int i = PowerupSpawnPoints.IndexOf(puspawn);
            if (puspawn.childCount == 0)
            {
                if (PowerupWasHere[i])
                {
                    PowerupGrace[i] = true;
                    PowerupWasHere[i] = false;
                    IEnumerator NoSpawn = EndGrace(i);
                    StartCoroutine(NoSpawn);
                }

                if (!PowerupGrace[i])
                {
                    int spawnhere = rnd.Next(0, PowerupSpawnRate);
                    
                    if (spawnhere == 0)
                    {
                        int whichpu = rnd.Next(0, Powerups.Length);
                        Instantiate(Powerups[whichpu], puspawn);
                    }
                }
            }
            else
            {
                PowerupWasHere[i] = true;
            }
        }
    }

    private IEnumerator EndGrace(int i)
    {
        yield return new WaitForSeconds(PowerupGraceLength);
        PowerupGrace[i] = false;
    }
}