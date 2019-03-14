using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class HazardSpawner : MonoBehaviour
{
    public List<Transform> BoxSpawnPoints;
    public List<Transform> SpikeSpawnPoints;
    public GameObject[] Auras;

    public GameObject ExplodingBox, FallingSpike;

    public int SpikeSpawnRate, BoxSpawnRate;
    public float HazardGraceLength;

    public List<bool> BoxWasHere, SpikeWasHere, BoxGrace, SpikeGrace;

    private void Start()
    {
        Auras = GameObject.FindGameObjectsWithTag("Auras");

        foreach (var child in GameObject.Find("Exploding Boxes").GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name.Contains("Spawn"))
            {
                BoxSpawnPoints.Add(child);
                BoxWasHere.Add(false);
                BoxGrace.Add(false);
            }
        }

        foreach (var child in GameObject.Find("Falling Spikes").GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name.Contains("Spawn"))
            {
                SpikeSpawnPoints.Add(child);
                SpikeWasHere.Add(false);
                SpikeGrace.Add(false);
            }
        }

        SpikeWasHere.Capacity = SpikeGrace.Capacity = SpikeSpawnPoints.Count;
    }

    private void FixedUpdate()
    {
        Auras = GameObject.FindGameObjectsWithTag("Auras");
        bool playerhere = false;

        Random rnd = new Random();

        foreach (Transform boxspawn in BoxSpawnPoints)
        {
            int i = BoxSpawnPoints.IndexOf(boxspawn);
            if (boxspawn.childCount == 0)
            {
                if (BoxWasHere[i])
                {
                    BoxGrace[i] = true;
                    BoxWasHere[i] = false;
                    IEnumerator NoSpawn = EndGrace("ExplodingBox", i);
                    StartCoroutine(NoSpawn);
                }
                
                if (!BoxGrace[i])
                {
                    int spawnhere = rnd.Next(0, BoxSpawnRate);

                    if (spawnhere == 0)
                    {
                        foreach (GameObject area in Auras)
                        {
                            if (area.GetComponent<Collider2D>().bounds.Contains(boxspawn.position))
                                playerhere = true;
                        }

                        if (!playerhere) Instantiate(ExplodingBox, boxspawn);
                    }
                }
            }
            else
            {
                BoxWasHere[i] = true;
            }
        }

        foreach (Transform spikespawn in SpikeSpawnPoints)
        {
            int i = SpikeSpawnPoints.IndexOf(spikespawn);
            if (spikespawn.childCount == 0)
            {
                if (SpikeWasHere[i])
                {
                    SpikeGrace[i] = true;
                    SpikeWasHere[i] = false;
                    IEnumerator NoSpawn = EndGrace("FallingSpike", i);
                    StartCoroutine(NoSpawn);
                }
                
                if (!SpikeGrace[i])
                {
                    int spawnhere = rnd.Next(0, SpikeSpawnRate);
                    
                    if (spawnhere == 0)
                    {
                        foreach (GameObject area in Auras)
                            if (area.GetComponent<Collider2D>().bounds.Contains(spikespawn.position))
                                playerhere = true;
                        if (!playerhere) Instantiate(FallingSpike, spikespawn);
                    }
                }
            }
            else
            {
                SpikeWasHere[i] = true;
            }
        }
    }

    private IEnumerator EndGrace(string hazard, int i)
    {
        yield return new WaitForSeconds(HazardGraceLength);
        if (hazard == "ExplodingBox") BoxGrace[i] = false;
        else if (hazard == "FallingSpike") BoxGrace[i] = false;
    }
}