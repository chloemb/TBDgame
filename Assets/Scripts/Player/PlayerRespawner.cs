using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    public List<Transform> PlayerSpawnPoints;
    public GameObject[] Players;
    public GameObject MovingPlatforms;
    private bool _mPlatform;

    // Start is called before the first frame update
    void Start()
    {
        _mPlatform = true;
        if (!FindObjectOfType<MovingPlatform>())
        {
            _mPlatform = false;
        }
        foreach (var child in GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name.Contains("Respawn"))
            {
                PlayerSpawnPoints.Add(child.gameObject.transform);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_mPlatform)
        {
            if (PlayerSpawnPoints[0].childCount == 0 && MovingPlatforms.transform.childCount == 0)
            {
                Instantiate(Players[0], PlayerSpawnPoints[0]);
            }

            if (PlayerSpawnPoints[1].childCount == 0 && MovingPlatforms.transform.childCount == 0)
            {
                Instantiate(Players[1], PlayerSpawnPoints[1]);
            }
        }
        else
        {
            if (PlayerSpawnPoints[0].childCount == 0)
            {
                Instantiate(Players[0], PlayerSpawnPoints[0]);
            }

            if (PlayerSpawnPoints[1].childCount == 0)
            {
                Instantiate(Players[1], PlayerSpawnPoints[1]);
            }
        }
        
    }
}