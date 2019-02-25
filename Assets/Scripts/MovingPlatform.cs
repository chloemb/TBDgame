using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Rigidbody2D _rb;
    public bool StartingLeftRight; // true for left, false for right
    public bool _leftRight;
    public float speed;
    public Vector2 startPos;
    public float changeValue; // Amount of distance the platform covers before switching
    // Use this for initialization
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _leftRight = StartingLeftRight;
        if(!GetComponent<MovingSpike>())
            startPos = _rb.position;
        Debug.Log(startPos.x);
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
                    _rb.velocity = new Vector2(speed * -1, 0);
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

                    _rb.velocity = new Vector2(speed, 0);
                }
            }
            else //starts going right
            {
                if (_leftRight)
                {
                    _rb.velocity = new Vector2(speed * -1, 0);
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

                    _rb.velocity = new Vector2(speed, 0);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Yatta");
             col.gameObject.GetComponent<Rigidbody2D>().velocity = (_leftRight) ? new Vector2(speed * -1, 0) : new Vector2(speed, 0);
            Debug.Log(col.gameObject.GetComponent<Rigidbody2D>().velocity);
        }
    }
}
