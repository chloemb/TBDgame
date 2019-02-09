using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public HealthManager[] Players;

    private Transform RespawnPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        Players = GameObject.FindObjectsOfType<HealthManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        foreach (HealthManager player in Players)
        {
            if (player.Health == 0)
            {
                Respawn(player);
            }
        }
        
    }

    private void Respawn(HealthManager player)
    {
        switch (player.gameObject.name)
        {
            case "Player 1":
                RespawnPoint = GameObject.Find("P1Respawn").transform;
                break;
            case "Player 2":
                RespawnPoint = GameObject.Find("P2Respawn").transform;
                break;
            default:
                RespawnPoint = GameObject.Find("P1Respawn").transform;
                break;
        }

        player.gameObject.transform.position = RespawnPoint.position;
        player.ResetHealth();
        Rigidbody2D playerRB = player.gameObject.GetComponent<Rigidbody2D>();
        playerRB.velocity = new Vector2(0f, 0f);
    }
}
