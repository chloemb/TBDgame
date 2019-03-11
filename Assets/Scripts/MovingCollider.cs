using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCollider : MonoBehaviour
{

    public List<Transform> PlayerSpawnPoints;

    private void Start()
    {
        foreach (var child in FindObjectsOfType<Transform>())
        {
            if (child.gameObject.name.Contains("Respawn"))
            {
                PlayerSpawnPoints.Add(child.gameObject.transform);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" && col.gameObject.name == "Player 1")
        {
            col.collider.transform.SetParent(PlayerSpawnPoints[1]);
        }
        else if (col.gameObject.tag == "Player" && col.gameObject.name == "Player 2")
        {
            col.collider.transform.SetParent(PlayerSpawnPoints[0]);
        }
    }
}
