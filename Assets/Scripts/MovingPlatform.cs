using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Rigidbody2D _rb;
    public bool StartingLeftRight; // true for left, false for right
    private bool _leftRight;
    public float speed;
    private Vector2 startPos;
    public float changeValue; // Amount of distance the platform covers before switching
    

    // Use this for initialization
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _leftRight = StartingLeftRight;
        startPos = _rb.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!(_rb.velocity.y > 0))
        {
            if (StartingLeftRight) //starts going left
            {
                if (_leftRight)
                {
                    _rb.MovePosition(_rb.position - new Vector2(speed, 0));
                    if (_rb.position.x <= startPos.x - changeValue)
                    {
                        _leftRight = false;
                    }
                }
                else
                {
                    if (_rb.position.x >= startPos.x)
                    {
                        _leftRight = true;
                    }

                    _rb.MovePosition(_rb.position + new Vector2(speed, 0));
                }
            }
            else //starts going right
            {
                if (_leftRight)
                {
                    _rb.MovePosition(_rb.position - new Vector2(speed, 0));
                    if (_rb.position.x <= startPos.x)
                    {
                        _leftRight = false;
                    }
                }
                else
                {
                    if (_rb.position.x >= startPos.x + changeValue)
                    {
                        _leftRight = true;
                    }

                    _rb.MovePosition(_rb.position + new Vector2(speed, 0));
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("On");
            col.collider.transform.SetParent(transform);
            col.collider.transform.GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }

    /*private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" && col.gameObject.name == "Player 1")
        {
            //col.collider.transform.SetParent(PlayerSpawnPoints[0]);
           // col.collider.transform.GetComponent<Rigidbody2D>().isKinematic = false;
        }
        else if (col.gameObject.tag == "Player" && col.gameObject.name == "Player 2")
        {
            //col.collider.transform.SetParent(PlayerSpawnPoints[1]);
            //col.collider.transform.GetComponent<Rigidbody2D>().isKinematic = false;
        }
    }*/
}
